module.exports = {
    extends: [
      "airbnb-typescript",
      "airbnb/hooks",
      "plugin:@typescript-eslint/recommended",
      "prettier",
      "prettier/react",
      "prettier/@typescript-eslint",
      "plugin:prettier/recommended"
    ],
    plugins: ["react", "@typescript-eslint", "jest"],
    env: {
      browser: true,
      es6: true,
      mocha: true
    },
    globals: {
      Atomics: "readonly",
      SharedArrayBuffer: "readonly",
    },
    parser: "@typescript-eslint/parser",
    parserOptions: {
      ecmaFeatures: {
        jsx: true,
      },
      ecmaVersion: 2020,
      sourceType: "module",
      project: "./tsconfig.json",
    },
    rules: {
      "linebreak-style": "off",
      "react/prefer-stateless-function": [0],
      "no-param-reassign": ["error", { "props": false }],
      "no-use-before-define": "off",
      "@typescript-eslint/no-use-before-define": "off",
      "import/no-extraneous-dependencies": ["error", {"devDependencies": true}],
      "prettier/prettier": [
        "error",
        {
          endOfLine: "auto",
        },
      ],
      "class-methods-use-this": [0],
      // note you must disable the base rule as it can report incorrect errors
      "no-shadow": "off",
      "@typescript-eslint/no-shadow": ["error"]
    },
  };