// Razzle config tweaks to make pnpm layout and symlinked helpers work with CRA-like checks.
module.exports = {
  modify: (config, { target, dev }, webpack) => {
    try {
      // Prevent webpack from resolving symlinks to their real paths. This keeps
      // module paths under the project `node_modules` root so ModuleScopePlugin
      // and other CRA checks don't think files are outside the project `src`.
      if (config && config.resolve) {
        config.resolve.symlinks = false;
      }
    } catch (e) {
      // ignore
    }
    return config;
  }
};
