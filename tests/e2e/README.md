# Browser regression suites

Self-checking browser tests for UI flows that unit tests can't cover. They drive a running local app with
[`playwright-cli`](https://github.com/microsoft/playwright-cli).

| Suite    | File                       | Covers |
|----------|----------------------------|--------|
| `guard`  | `unsaved-changes-guard.js` | Leave/unload prompt on the Recipe, Category, Grocery Item/Aisle/Store and Storage Location edit pages, including right after creating a record. |
| `images` | `recipe-images.js`         | Recipe image upload (single and multiple), rejection of non-images, pin, delete, carousel position, failed upload. |

## Running

1. Start the app locally (the `watch-web` VS Code task).
2. Install the CLI once: `npm install -g @playwright/cli@latest`.
3. Run:

```powershell
./tests/e2e/run.ps1                  # all suites
./tests/e2e/run.ps1 -Suite images    # one suite
./tests/e2e/run.ps1 -BaseUrl https://localhost:5001
```

The runner prints each check as `PASS`/`FAIL` and exits non-zero if any check fails or a suite doesn't finish.

> **Local only.** The suites create real records named `zz-e2e-<timestamp>` and delete them when they finish.
> If a run is interrupted, delete any leftover `zz-e2e-*` records by hand. Never run them against Test or Production.

## How it works

- Each suite is one `async (page) => { ... }` function run with `playwright-cli run-code`.
- Existing records to edit are discovered through the API, so no specific data is required beyond at least one
  recipe, category, grocery item, aisle, store and storage location.
- The CLI can return before a script finishes (native `beforeunload` dialogs make it report a pending modal), so
  scripts save their results to `localStorage` under `e2eResults` and the runner polls that until it sees `DONE`.
- A fresh browser is opened per suite so one suite can't leave a dirty page behind for the next.

## Adding a suite

1. Copy the structure of an existing script: log with `PASS`/`FAIL` lines, `flush()` results, end with `DONE`.
2. Register it in the `$suites` table in `run.ps1` and the `-Suite` `ValidateSet`.
3. Make it clean up everything it creates, and prefer in-place cleanup over navigating away from a dirty page.

## Known quirks

- When several images upload at once, which one the carousel lands on depends on which response arrives last, so
  the multi-upload check doesn't assert the active slide.
