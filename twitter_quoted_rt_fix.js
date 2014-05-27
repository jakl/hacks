// ==UserScript==
// @name       Fix Quoted RT
// @namespace  https://twitter.com
// @version    0.1
// @description  Remove repeated quotes from quoted RT conversation thread
// @match      https://twitter.com/*
// @copyright  2012+, @jakl
// ==/UserScript==

replies = document.querySelectorAll('.stream-items .tweet-text')
for(i = 0; i < replies.length; i++) {
  reply = replies[i]
  // delete all text inside the angled quotes, typical of quoted RT's
  reply.innerHTML = reply.innerHTML.replace(/“.+?”/, '')
}
