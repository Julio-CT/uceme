---
name: Blog add/edit fixes
overview: Run a full solution build to surface compile/test failures, then fix the admin blog modal so image state resets between posts and uploads behave correctly. Optionally harden server-side image naming and slug handling.
todos:
  - id: build-test
    content: dotnet build (solution, zero errors); dotnet test + ClientApp npm test — all passing
    status: completed
  - id: unit-tests
    content: Unit tests for all changed code (e.g. BlogService/upload naming if touched, AddPostModal behavior)
    status: completed
  - id: integration-tests
    content: Integration tests with Shouldly + WireMock where HTTP/boundaries apply; add packages to test project if missing
    status: completed
  - id: lint-format
    content: dotnet format / analyzers clean; ClientApp npm run lint, format, stylelint — fix all issues
    status: completed
  - id: add-post-modal-reset
    content: Reset imgSrc/uploadSuccess/photo (and related) on success + add open; reset uploadSuccess in setFile
    status: completed
  - id: add-post-modal-useeffect
    content: Move post→state sync from render to useEffect([post, modal])
    status: completed
  - id: meta-json-key
    content: "Align submit JSON key with PostRequest: metaDescription"
    status: completed
  - id: optional-upload-naming
    content: "Optional: GUID/timestamp-based blog image names + empty-table guard"
    status: completed
isProject: false
---

# Blog add/edit review and build

## What we found

### 1. Repeated images on new posts (primary bug)

[`PostManager.tsx`](src/Uceme.UI/ClientApp/src/components/admin/PostManager.tsx) mounts **two** [`AddPostModal`](src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.tsx) instances: one for edit (`post={markedPost}`) and one for add (**no `post` prop** → `null`). The add instance stays mounted when the modal closes; only `isOpen` toggles.

After a successful publish, [`resetForm`](src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.tsx) only clears title, slug, text, and caption. It does **not** reset:

- `imgSrc` (the `foto` value sent to the API)
- `uploadSuccess` (disables “Subir imagen” while `true`)
- `photo` (file input state)
- other fields (meta, SEO, date, `id`)

On the next “Nuevo Post”, `post` and `currentPost` are both `undefined`, so the `if (post !== currentPost)` sync block **never runs**, and the previous post’s `imgSrc` remains → **new row reuses the previous image filename**.

Secondary UX bug: choosing a new file in [`setFile`](src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.tsx) does not set `uploadSuccess` back to `false`, so after one successful upload the user cannot upload a replacement image in the same session.

### 2. React pattern risk in `AddPostModal`

Lines 265–283 run multiple `setState` calls **during render** when `post !== currentPost`. That is unsupported and can cause inconsistent updates; this should move to a `useEffect` keyed on `post` (and ideally `modal`).

### 3. Server-side image naming

[`GetNextPostImage`](src/Uceme.Library/Services/BlogService.cs) uses `max(idBlog) + 1` to name the next file. That **predicts** the next identity value. It usually matches the row created next, but:

- Concurrent uploads can target the same filename and **overwrite** the file on disk.
- An **empty** `Blog` table would throw on `.First()`.

[`BlogController.OnPostUploadAsync`](src/Uceme.API/Controllers/BlogController.cs) builds `Blog{next}.png.webp`-style names via `SaveToWebP`.

### 4. Slug behavior (context for your `CheckUniqueSlug` change)

[`AddPost`](src/Uceme.Library/Services/BlogService.cs) still calls [`CheckUniqueSlug`](src/Uceme.Library/Services/BlogService.cs), which recursively prefixes `"Mas "` — odd for SEO and can surprise authors. [`UpdatePost`](src/Uceme.Library/Services/BlogService.cs) no longer mutates slug (matches your intent to stop unexpected slug changes on edit). No further change required unless you want stricter validation (reject duplicate slug instead of mutating).

### 5. Client payload vs contract

[`PostRequest`](src/Uceme.Model/DataContracts/PostRequest.cs) expects JSON property `metaDescription`. The modal sends `metadescription` in the request body. Depending on ASP.NET Core JSON options, this may or may not bind; aligning the key to `metaDescription` removes ambiguity.

---

## Recommended implementation order

1. **Quality gates first and last (see [Definition of done](#definition-of-done-this-plan-and-repo-default) below)**  
   - `dotnet build` (zero errors), full `dotnet test`, ClientApp `npm test` where UI changes apply.  
   - Lint/format: `dotnet format` + analyzers; ClientApp `npm run lint`, `npm run format`, `npm run stylelint`.  
   - New or changed behavior: unit tests; HTTP/boundary scenarios: integration tests with **Shouldly** + **WireMock** (add packages to the test project if needed).

2. **Fix `AddPostModal` state lifecycle**  
   - Extend `resetForm` (or add `resetForNewPost`) to clear: `imgSrc`, `photo`, `uploadSuccess`, `id`, and optionally meta/SEO/date to defaults — and call it after successful submit **and** when the add modal opens (`modal` becomes true with no `post`).  
   - In `setFile`, when a new file is chosen: `setUploadSuccess(false)` (and optionally clear `imgSrc` until upload succeeds).  
   - Replace render-time `post !== currentPost` sync with `useEffect(() => { ... }, [post, modal])` to load edit data or reset for new post.

3. **Optional server hardening**  
   - Change upload naming to something non-colliding (e.g. `Blog{Guid.NewGuid():N}.webp` or `Blog{DateTime.UtcNow:yyyyMMddHHmmss}_{random}.webp`) and store that filename in `foto` — removes race between `GetNextPostImage` and concurrent editors.  
   - Guard `GetNextPostImage` for empty table or remove it if switching to GUID names.

4. **Small contract fix**  
   - In the modal submit payload, use `metaDescription` as the property name to match [`PostRequest`](src/Uceme.Model/DataContracts/PostRequest.cs).

---

## Files to touch

| Area | File |
|------|------|
| Main fix | [`src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.tsx`](src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.tsx) |
| Tests | [`AddPostModal.test.tsx`](src/Uceme.UI/ClientApp/src/components/admin/AddPostModal.test.tsx) — extend to assert reset after publish / reopen |
| Optional API | [`BlogController.cs`](src/Uceme.API/Controllers/BlogController.cs), [`BlogService.cs`](src/Uceme.Library/Services/BlogService.cs) |

No change required in [`BlogController.AddPost`](src/Uceme.API/Controllers/BlogController.cs) routing (add vs update by `idBlog`) for the image bug; the failure mode is client state.

---

## Definition of done (this plan and repo default)

Work is complete only when **all** of the following are true:

1. **Unit tests**: Every changed or new logical path is covered by unit tests (extend existing suites where appropriate).
2. **Integration tests**: Where the feature hits HTTP or external services, add or update integration tests using **Shouldly** for assertions and **WireMock** for stubbing outbound HTTP. If the repo has no WireMock/Shouldly in the right test project yet, add package references and baseline tests as part of this delivery.
3. **Lint and format**: No outstanding linter issues — .NET analyzers + `dotnet format` as applicable; for ClientApp, `npm run lint`, `npm run format`, and `npm run stylelint` (see [`package.json`](src/Uceme.UI/ClientApp/package.json)).
4. **Build**: Solution builds with **no errors**.
5. **All tests green**: Full `dotnet test` and ClientApp test script(s) pass.

> **Repo-wide rule**: These gates are also captured in [`.cursor/rules/plan-delivery-quality-gates.mdc`](.cursor/rules/plan-delivery-quality-gates.mdc) (`alwaysApply: true`) so future plans inherit the same completion criteria unless the user explicitly scopes them out.
