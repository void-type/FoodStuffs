import { readFileSync, writeFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = fileURLToPath(new URL('.', import.meta.url));
const apiDir = resolve(__dirname, '../src/api');

// List of files to process
const filesToFix = [
  resolve(apiDir, 'Api.ts'),
  resolve(apiDir, 'data-contracts.ts'),
  resolve(apiDir, 'http-client.ts'),
];

console.log('🔧 Fixing API files...');

for (const filePath of filesToFix) {
  try {
    let content = readFileSync(filePath, 'utf8');

    // Remove @ts-nocheck comment
    content = content.replace(/\/\/ @ts-nocheck\s*\n/g, '');

    // Also remove /* eslint-disable */ and /* tslint:disable */ if they exist
    content = content.replace(/\/\* eslint-disable \*\/\s*\n/g, '');
    content = content.replace(/\/\* tslint:disable \*\/\s*\n/g, '');

    // Fix type imports for verbatimModuleSyntax
    if (filePath.includes('Api.ts')) {
      // Replace regular imports with type-only imports for the types
      content = content.replace(
        /import \{([^}]+)\} from ["']\.\/data-contracts["'];/,
        (match, imports) => {
          const trimmedImports = imports.trim();
          return `import type {${trimmedImports}} from "./data-contracts";`;
        },
      );

      // Fix the http-client import to separate types and values
      content = content.replace(
        /import \{ ContentType, HttpClient, RequestParams \} from ["']\.\/http-client["'];/,
        'import { ContentType, HttpClient } from "./http-client";\nimport type { RequestParams } from "./http-client";',
      );
    }

    writeFileSync(filePath, content, 'utf8');
    console.log(`✅ Fixed: ${filePath}`);
  } catch (error) {
    console.warn(`⚠️  Could not process ${filePath}:`, error.message);
  }
}

console.log('✅ API files have been fixed!');
