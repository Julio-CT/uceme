#!/usr/bin/env node
// Create symlinks from pnpm's package store into the project's node_modules
// so tools that resolve realpaths (like CRA) don't see files outside the project
// src/ directory. This is defensive for pnpm layouts.
const fs = require('fs');
const path = require('path');

function log() { console.log.apply(console, arguments); }

function findAllPnpmDirs(root) {
  const pnpmDir = path.join(root, 'node_modules', '.pnpm');
  if (!fs.existsSync(pnpmDir)) return [];
  const entries = fs.readdirSync(pnpmDir);
  return entries.filter((d) => d.indexOf('@babel+runtime@') === 0 || d.indexOf('@babel+runtime%40') === 0 || (d.indexOf('@babel+runtime') !== -1 && d.includes('runtime'))).map((d) => path.join(pnpmDir, d, 'node_modules', '@babel', 'runtime'));
}

function ensureSymlinks() {
  const root = process.cwd();
  const sources = findAllPnpmDirs(root).filter((p) => fs.existsSync(p));
  if (!sources.length) {
    log('createPnpmSymlinks: no pnpm-installed @babel/runtime directories found; skipping.');
    return 0;
  }

  const destHelpers = path.join(root, 'node_modules', '@babel', 'runtime', 'helpers', 'esm');
  fs.mkdirSync(destHelpers, { recursive: true });

  sources.forEach((source) => {
    const srcHelpers = path.join(source, 'helpers', 'esm');
    if (!fs.existsSync(srcHelpers)) {
      log('createPnpmSymlinks: source helpers dir not found for', source);
      return;
    }
    const files = fs.readdirSync(srcHelpers);
    files.forEach((f) => {
      const srcFile = path.join(srcHelpers, f);
      const destFile = path.join(destHelpers, f);
      try {
        if (fs.existsSync(destFile)) {
          const stat = fs.lstatSync(destFile);
          if (stat.isSymbolicLink()) {
            const link = fs.readlinkSync(destFile);
            if (path.resolve(path.dirname(destFile), link) === path.resolve(srcFile)) {
              return;
            }
          }
          fs.unlinkSync(destFile);
        }
        fs.symlinkSync(srcFile, destFile);
        log('createPnpmSymlinks: linked', destFile, '->', srcFile);
      } catch (err) {
        log('createPnpmSymlinks: failed to link', destFile, err && err.message);
      }
    });
  });

  return 0;
}

// Create a compatibility alias for older pnpm-layout paths that some tools
// may resolve to (e.g. @babel+runtime@7.23.7). If a specific version folder
// doesn't exist under .pnpm but we have another version, create a minimal
// aliased folder pointing to the available version so absolute resolves
// succeed.
function ensureCompatibilityAlias(version) {
  const root = process.cwd();
  const pnpmRoot = path.join(root, 'node_modules', '.pnpm');
  const aliasDir = path.join(pnpmRoot, `@babel+runtime@${version}`);
  if (fs.existsSync(aliasDir)) return;
  // Find any existing @babel+runtime folder under .pnpm
  const entries = fs.existsSync(pnpmRoot) ? fs.readdirSync(pnpmRoot) : [];
  const existing = entries.find((d) => d.indexOf('@babel+runtime@') === 0 || d.indexOf('@babel+runtime') !== -1);
  if (!existing) return;
  const source = path.join(pnpmRoot, existing, 'node_modules', '@babel', 'runtime');
  const aliasHelpersSrc = path.join(source, 'helpers', 'esm');
  if (!fs.existsSync(aliasHelpersSrc)) return;

  const aliasHelpersDest = path.join(aliasDir, 'node_modules', '@babel', 'runtime', 'helpers', 'esm');
  fs.mkdirSync(aliasHelpersDest, { recursive: true });
  const files = fs.readdirSync(aliasHelpersSrc);
  files.forEach((f) => {
    const srcFile = path.join(aliasHelpersSrc, f);
    const destFile = path.join(aliasHelpersDest, f);
    try {
      if (fs.existsSync(destFile)) return;
      fs.symlinkSync(srcFile, destFile);
      log('createPnpmSymlinks: created compatibility link', destFile, '->', srcFile);
    } catch (e) {
      // ignore
    }
  });
}

try {
  const code = ensureSymlinks();
  process.exit(code);
} catch (e) {
  console.error('createPnpmSymlinks: unexpected error', e && e.stack ? e.stack : e);
  process.exit(1);
}
