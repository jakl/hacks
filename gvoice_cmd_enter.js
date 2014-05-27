// ==UserScript==
// @name       Google Voice Cmd Enter
// @namespace  https://www.google.com/voice*
// @version    0.1
// @description  Use cmd+enter to submit text message
// @match      https://www.google.com/voice*
// @copyright  2012+, @jakl
// ==/UserScript==

var script=document.createElement('script');
script.src = 'https://code.jquery.com/jquery-2.0.0.min.js'
document.head.appendChild(script)

$('.gc-message-sms-reply textarea').keydown(function(e){if(e.keyCode == 13 && (e.metaKey || e.ctrlKey)) $(this).parent().find('.gc-message-sms-send').click()})
// .click() .mousedown() .mouseup() don't work, why don't they work!
