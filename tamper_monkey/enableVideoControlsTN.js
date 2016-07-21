// ==UserScript==
// @name         TreesNetwork video controls
// @namespace    https://www.treesnetwork.com/
// @version      1.0
// @description  Make the timeout longer (below) if this doesn't work, to compensate for page load speed.
// @author       @jakl
// @match        https://www.treesnetwork.com/
// @grant        none
// ==/UserScript==

(function() {
    'use strict';
    var showVideoControls = function() {
        var video = document.querySelector('.video');
        var clickCatcher = document.querySelector(".player-catch-all");

        clickCatcher.remove();

        video.style.pointerEvents = "initial";
        video.controls = true;
    };

    // Make this timeout longer if it doesn't work, like 5000 maybe.
    // (Sorry window.onload doesn't work.)
    window.setTimeout(showVideoControls, 2000);
})();
