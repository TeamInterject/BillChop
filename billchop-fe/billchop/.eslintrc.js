module.exports = {
    extends: [
      "airbnb-typescript",
      "airbnb/hooks",
      "plugin:@typescript-eslint/recommended",
      "plugin:jest/recommended",
      "prettier",
      "prettier/react",
      "prettier/@typescript-eslint",
      "plugin:prettier/recommended"
    ],
    plugins: ["react", "@typescript-eslint", "jest"],
    env: {
      browser: true,
      es6: true,
      jest: true,
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
      "prettier/prettier": [
        "error",
        {
          endOfLine: "auto",
        },
      ]
    },
  };