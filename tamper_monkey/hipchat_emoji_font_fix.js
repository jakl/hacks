// ==UserScript==
// @name       Hipchat should render emoji in chrome
// @namespace  https://twitter.hipchat.com/chat
// @version    0.1
// @description  Change the hipchat font to support emoji
// @match      https://twitter.hipchat.com/chat
// @copyright  2012+, You
// ==/UserScript==

var style = document.createElement('style');
style.type = 'text/css';
style.innerHTML = '* { font-family: arial,sans-serif; }';
document.getElementsByTagName('head')[0].appendChild(style);
