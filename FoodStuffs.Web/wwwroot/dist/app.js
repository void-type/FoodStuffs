/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

﻿__webpack_require__(3);
__webpack_require__(4);
__webpack_require__(5);

/***/ }),
/* 1 */
/***/ (function(module, exports) {

﻿var appState = {
    recipes: null,
    currentRecipe: null,
    messages: [],
    isError: false,

    clearMessages: function () {
        appState.messages = null;
    },

    success: function (messages) {
        appState.isError = false;
        appState.messages = messages;
    },

    error: function (messages) {
        appState.isError = true;
        appState.messages = messages;
    },

    list: function () {
        appState.clearMessages();

        axios.get("api/recipes/list")
            .then(function (response) {
                appState.recipes = response.data.items;
            });
    },

    create: function (recipe) {
        appState.clearMessages();

        axios.put("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    },

    update: function (recipe) {
        appState.clearMessages();

        axios.post("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    },

    delete: function (recipeId) {
        appState.clearMessages();

        axios.delete("api/recipes", recipeId)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    }
};

appState.list();

module.exports = appState;

/***/ }),
/* 2 */,
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

﻿var appState = __webpack_require__(1);

const component = new Vue({
    el: "#message-center",
    data: appState
});

module.exports = component;

/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

﻿var appState = __webpack_require__(1);

const component = new Vue({
    el: "#recipe-form",
    data: appState,
    methods: {
        cancel: function () {
            appState.currentRecipe = null;
        }
    }
});

module.exports = component;

/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

﻿var appState = __webpack_require__(1);

const component = new Vue({
    el: "#recipe-table",
    data: appState,
    methods: {
        select: function (recipe) {
            appState.currentRecipe = recipe;
        },
        newRecipe: function () {
            appState.currentRecipe = {};
        }
    }
});

module.exports = component;

/***/ })
/******/ ]);