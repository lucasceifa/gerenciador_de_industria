/* eslint-disable no-undef */
const flowbiteReact = require("flowbite-react/plugin/tailwindcss");

/* eslint-disable no-undef */
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
    "node_modules/flowbite-react/lib/esm/**/*.js",
    ".flowbite-react\\class-list.json"
  ],
  theme: {
    extend: {},
  },
  plugins: [require("flowbite/plugin"), flowbiteReact],
};