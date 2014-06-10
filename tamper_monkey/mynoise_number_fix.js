// ==UserScript==
// @name       MyNoise Fix for Cmd+1-9 in Chrome
// @namespace  http://mynoise.net
// @version    0.1
// @description  Set volume to max when switching tabs, to override "broken" default
// @match      http://mynoise.net/*
// @copyright  2012+, @jakl
// ==/UserScript==

$(document).keydown(function(e) {
    if(e.keyCode >= 48 && e.keyCode <= 57){
        setPreset(1,1,1,1,1,1,1,1,1,1,"All")
        toggleMute()
        toggleMute()
    }
})
