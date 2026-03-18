import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  globalSetup: './e2e/global-setup.ts',
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: [['html'], ['list']],
  use: {
    baseURL: 'https://localhost:5001',
    ignoreHTTPSErrors: true,
    trace: 'on-first-retry',
    testIdAttribute: 'data-test-id',
    ...devices['Desktop Chrome'],
  },
  projects: [
    {
      name: 'smoke',
      testMatch: 'home.spec.ts',
    },
    {
      name: 'crud',
      testIgnore: 'home.spec.ts',
      dependencies: ['smoke'],
    },
  ],
});
