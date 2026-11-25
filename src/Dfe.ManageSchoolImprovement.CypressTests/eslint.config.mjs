import js from "@eslint/js";
import tseslint from "@typescript-eslint/eslint-plugin";
import tsparser from "@typescript-eslint/parser";
import cypress from "eslint-plugin-cypress/flat";
import globals from "globals";

export default [
  {
    ignores: [
      "node_modules/**",
      "cypress/reports/**",
      "cypress/screenshots/**",
      "cypress/videos/**",
      "mochareports/**",
    ],
  },
  js.configs.recommended,
  cypress.configs.recommended,
  {
    files: ["scripts/**/*.js"],
    languageOptions: {
      ecmaVersion: "latest",
      sourceType: "commonjs",
      globals: { ...globals.node },
    },
  },
  {
    files: ["cypress/plugins/**/*.js"],
    languageOptions: {
      ecmaVersion: "latest",
      sourceType: "module",
      globals: { ...globals.node },
    },
  },
  {
    files: ["**/*.ts"],
    languageOptions: {
      parser: tsparser,
      ecmaVersion: "latest",
      sourceType: "module",
      globals: { ...globals.node, ...globals.mocha },
    },
    plugins: { "@typescript-eslint": tseslint },
    rules: {
      ...tseslint.configs["eslint-recommended"].overrides[0].rules,
      ...tseslint.configs.recommended.rules,
      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/no-require-imports": "off",

      "@typescript-eslint/no-unused-vars": [
        "error",
        { argsIgnorePattern: "^_", varsIgnorePattern: "^_" },
      ],
    },
  },
  {
    files: ["cypress/support/**/*.ts"],
    rules: {
      "@typescript-eslint/no-namespace": "off",
    },
  },
  {
    files: ["cypress.config.ts"],
    languageOptions: {
      sourceType: "module",
      globals: { ...globals.node },
    },
  },
];
