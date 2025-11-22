import antfu from '@antfu/eslint-config';

export default antfu({
  formatters: true,
  vue: true,
  typescript: true,
  stylistic: {
    semi: true,
    quotes: 'single',
    commaDangle: 'always-multiline',
  },
  rules: {
    // Enforce curly braces for all control statements
    'curly': ['error', 'all'],
    // Enforce consistent brace style
    'style/brace-style': ['error', '1tbs', { allowSingleLine: false }],
    // Enforce semicolons
    'style/semi': ['error', 'always'],
    // Enforce trailing commas in multiline
    'style/comma-dangle': ['error', 'always-multiline'],
    // Enforce consistent quote style
    'style/quotes': ['error', 'single'],
  },
  ignores: [
    // Generated API files
    '**/src/api/**',
  ],
});
