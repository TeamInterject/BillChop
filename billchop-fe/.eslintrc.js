module.exports = {
  parser: '@typescript-eslint/parser', // Specifies the ESLint parser
  extends: [
    'plugin:react/recommended', // Uses the recommended rules from @eslint-plugin-react
    'plugin:@typescript-eslint/recommended', // Uses the recommended rules from the @typescript-eslint/eslint-plugin
  ],
  env: {
    browser: true,
    es6: true,
    mocha: true
  },
  parserOptions: {
    ecmaVersion: 2018, // Allows for the parsing of modern ECMAScript features
    sourceType: 'module', // Allows for the use of imports
    ecmaFeatures: {
      jsx: true, // Allows for the parsing of JSX
    },
    project: "./tsconfig.json",
  },
  rules: {
    'react/jsx-max-props-per-line': ['error', { maximum: 1, when: 'multiline' }],
    'arrow-parens': ['error', 'always'],
    'react/jsx-closing-bracket-location': [1, 'line-aligned'],
    'comma-dangle': ['error', 'always-multiline'],
    quotes: ['error', 'double'],
    semi: ['error', 'always'],
    indent: ['error', 2],
    'max-len': ['warn', { code: 160 }],
    'linebreak-style': 0,
    eqeqeq: ['error', 'always'],
    'no-multi-spaces': 'error',
    'no-multiple-empty-lines': 'error',
    "semi": "off",
    "@typescript-eslint/semi": ["error"]
    // Place to specify ESLint rules. Can be used to overwrite rules specified from the extended configs
    // e.g. "@typescript-eslint/explicit-function-return-type": "off",
  },
  settings: {
    react: {
      version: 'detect', // Tells eslint-plugin-react to automatically detect the version of React to use
    },
  },
};