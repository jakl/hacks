// ==UserScript==
// @name TTC Improver
// @namespace https://translate.twitter.com/
// @version 1.4.8
// @author @insideRobb
// @description Improve your UX on Twitter Translation Center. (This is an unmaintained copy from Oct 25th, 2015)
// @website http://robb.be/download/
// @downloadURL http://robb.be/ttc/improver.user.js
// @updateURL http://robb.be/ttc/improver.meta.js
// @supportURL http://robb.be/download/?forum=report-bug
// @include https://translate.twitter.com/*
// @require https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js
// @released 2014-05-21
// @updated 2015-10-23
// @copyright 2014+,insideRobb
// @grant none
// ==/UserScript==
 
function TranslateTwitterAdapt() {
    jqttci = jQuery.noConflict(true);
    function cache(page) {
        var hours = new Date().getHours();
        if (hours < 10) {
            hours = '0' + hours;
        }
        var mins = new Date().getMinutes();
        if (mins < 10) {
            mins = '0' + mins;
        }
        var currenttime = hours + '' + mins;
        if (sessionStorage.getItem('time' + page) === null) {
            sessionStorage.setItem('time' + page, currenttime);
        }
        var cachable = currenttime - sessionStorage.getItem('time' + page);
        if ((cachable >= 30) || (cachable === 0 && sessionStorage.getItem('cache' + page) === null)) {sessionStorage.setItem('time' + page, currenttime); return false;}
        else {return true;}
    }
    function ttciinterval(selector) {
        var interval = setInterval(function() {
            if (jqttci("#translations .table tbody tr:nth-child(" + selector + ")").length) {
                clearInterval(interval);
                var ttcistringtranslation = document.querySelectorAll(".translation p");
                for (i = 0; i < ttcistringtranslation.length; i++) {
                    if(((ttcistringtranslation[i].innerHTML.slice(-2) == "..")||(ttcistringtranslation[i].innerHTML.substr(-1) == "✍")||(ttcistringtranslation[i].innerHTML.substr(-1) == "\u1160")||(ttcistringtranslation[i].innerHTML.substr(-1) == "\u115f")||(/\s+$/.test(ttcistringtranslation[i].innerHTML)))&&(ttcistringtranslation[i].innerHTML.slice(-3) != "...")) {
                        ttcistringtranslation[i].innerHTML+=" <b style='color: red'>Unnecessary symbol</b>";
                    }
                }
            }
        }, 100);
    }
    function forumdashboard(tag) {
        var value;
        if(tag == "*" || tag == "**") {
            value = prompt("Please enter text");
            if(value !== null) {jqttci("textarea.text").val(jqttci("textarea.text").val() + tag + value + tag + " ");}
        }
        else if(tag == "* ") {
            value = prompt("Please enter text for unordered list item and then press ENTER");
            if(value !== null) {jqttci("textarea.text").val(jqttci("textarea.text").val() + tag + value);}
        }
        else if(tag == "link") {
            value = prompt("Please enter text for link");
            var link = prompt("Please enter URL", "http://");
            if(value !== null && link !== null) {jqttci("textarea.text").val(jqttci("textarea.text").val() + "[" + value + "]" + "(" + link + ") ");}
        }
        else if(tag !== null) {
            value = prompt("Please enter text for header and press ENTER");
            if(value !== null) {jqttci("textarea.text").val(jqttci("textarea.text").val() + tag + " " + value);}
        }
    }
    function ttciicount(ttcicountsource, ttcicountnum, ttcicountstorage) {
        jqttci('#improverdata').load('/user/' + getusername + '/' + ttcicountsource + ' .container .profile-activity ul', function () {
            var improvervalue = (parseInt(jqttci('#improverdata ul:last-child li:nth-last-child(2) a').text())) * 20;
            if (isNaN(improvervalue)) {
                improvervalue = document.querySelectorAll("#improverdata .translation-timeline li").length;
            }
            jqttci('.stats a:nth-of-type(' + ttcicountnum + ') .stat-value').text(improvervalue);
            sessionStorage.setItem(ttcicountstorage + '_' + getusername, improvervalue);
        });
    }
    function lastprofileac(username, where) {
        jqttci('#improverdata').load('/user/' + username + ' .container', function () {
            var mod = {
                name: jqttci("#improverdata .profile-image").attr('alt'),
                avatar: jqttci("#improverdata .profile-image").attr('src'),
                date: jqttci("#improverdata .translation-timeline .translation-timeline-item:first-child .translation-timeline-item-meta").text(),
                status: jqttci("#improverdata .translation-timeline .translation-timeline-item:first-child .string-meta span:first-child")[0].outerHTML,
                link: jqttci("#improverdata .translation-timeline .translation-timeline-item:first-child > a:last-of-type").attr('href'),
                content: jqttci("#improverdata .translation-timeline .translation-timeline-item:first-child .translation-timeline-item-translation").html()
            };
            jqttci(where).append('<div class="dashboard-center-list-item table-row"><a href="/user/' + username + '"><div class="dashboard-center-item-avatar table-cell"><img alt="' + mod.name + '" class="pull-left img-rounded" src="' + mod.avatar + '" style="width: 35px; height: 35px; max-width: 35px"></div></a><div class="dashboard-list-item-left adjacent-to-avatar table-cell" style="width: 73%;"><div class="dashboard-user-info clearfix"><a href="/user/' + username + '"><div class="black pull-left clearfix">' + mod.name + '</div></a><a href="https://twitter.com/' + username + '"><div class="gray-light pull-left elbow-room clearfix">@' + username + '</div></a></div><div class="dashboard-activity-info clearfix">' + mod.status + '<a href="' + mod.link + '"><h4 class="translation-timeline-item-translation" style="display: inline"> ' + mod.content + '</h4></a></div></div><div class="dashboard-list-item-right table-cell text-right gray-medium">' + mod.date + '</div></div>');
            jqttci(where + " .spinner-loader").remove();
        });
    }
 
    function buildmodsdash() {
        var modsscreenname = document.querySelectorAll('.dashboard-moderator .dashboard-moderator-username');
        for (i = 0; i < modsscreenname.length; i++) {
            var modsusername = modsscreenname[i].textContent.replace("@", "");
            lastprofileac(modsusername, ".moderatorsdash");
        }
    }
 
    function buildforumdash() {
        var ttcimproverdates = [];
        jqttci('#improverdata').load('/forum/forums/' + ttcilanguage + ' .topics .topic > td[class*="e"]', function () {
            var ttciforumdates = {
                ints: document.querySelectorAll("#improverdata .latest-post a"),
                titles: document.querySelectorAll("#improverdata .subject a")
            };
            for (i = 0; i < ttciforumdates.ints.length; i++) {
                var ttciforumdatesmeta = {
                    num: ttciforumdates.ints[i].textContent.split(' ')[0], // number eg 1
                    time: ttciforumdates.ints[i].textContent.split(' ')[1], // unit eg minute
                    ago: ttciforumdates.ints[i].textContent.split(' ')[2], // for almost, over and about
                    nick: ttciforumdates.ints[i].textContent.split(' ')[5], // insideRobb
                    link: ttciforumdates.ints[i].href, // https
                    title: ttciforumdates.titles[i].textContent, // Introduce yourself
                    date: {}
                };
                //conversion in minutes
                if(ttciforumdatesmeta.time == "month" || ttciforumdatesmeta.time == "months") {ttciforumdatesmeta.date = ttciforumdatesmeta.num * 43200;}
                else if(ttciforumdatesmeta.time == "day" || ttciforumdatesmeta.time == "days") {ttciforumdatesmeta.date = ttciforumdatesmeta.num * 1440;}
                else if (ttciforumdatesmeta.time == "hour" || ttciforumdatesmeta.time == "hours") {ttciforumdatesmeta.date = ttciforumdatesmeta.num * 60;}
                else if ((ttciforumdatesmeta.num == "about") && (ttciforumdatesmeta.ago == "hour" || ttciforumdatesmeta.ago == "hours")) {ttciforumdatesmeta.date = ttciforumdatesmeta.time * 60; ttciforumdatesmeta.nick = ttciforumdates.ints[i].textContent.split(' ')[6]; ttciforumdatesmeta.time = ttciforumdatesmeta.time + " " + ttciforumdatesmeta.ago;}
                else if (ttciforumdatesmeta.time == "minute" || ttciforumdatesmeta.time == "minutes") {ttciforumdatesmeta.date = ttciforumdatesmeta.num;}
                else if (ttciforumdatesmeta.time == "than") {ttciforumdatesmeta.date = 1; ttciforumdatesmeta.nick = ttciforumdates.ints[i].textContent.split(' ')[7]; ttciforumdatesmeta.time = "than 1 minute";}
                else if(ttciforumdatesmeta.num == "about" && ttciforumdatesmeta.ago == "month") {ttciforumdatesmeta.date = 43200; ttciforumdatesmeta.nick = ttciforumdates.ints[i].textContent.split(' ')[6]; ttciforumdatesmeta.time = ttciforumdatesmeta.time + " " + ttciforumdatesmeta.ago;}
                else {ttciforumdatesmeta.date = 2000000000000; ttciforumdatesmeta.time = ttciforumdatesmeta.time + ' ' + ttciforumdatesmeta.ago;} // valid for months and years - over, about, almost
                ttcimproverdates[i]={ago:ttciforumdatesmeta.date, num:ttciforumdatesmeta.num, unit:ttciforumdatesmeta.time, link:ttciforumdatesmeta.link, nick:ttciforumdatesmeta.nick, title:ttciforumdatesmeta.title};
            }
 
            ttcimproverdates.sort(function(a, b){
                return a.ago-b.ago;
            });
            for (i = 0; i < 3; i++) {
                if(i == 0) {
                    jqttci(".forumdash .spinner-loader").remove();
                }
                jqttci(".forumdash").append('<div class="dashboard-center-list-item table-row clearfix"><div class="dashboard-list-item-left adjacent-to-icon table-cell" style="width: 84.5%"><div class="dashboard-priority dashboard-priority-description clearfix"><a href="/user/' + ttcimproverdates[i].nick + '"><div class="black" style="display: inline">@' + ttcimproverdates[i].nick + '</div></a> <div class="gray-light" style="display: inline">replied</div></div><div class="dashboard-manager-user-info clearfix"><div class="clearfix"><div class="dashboard-user-name clearfix">to <a href="' + ttcimproverdates[i].link + '">' + ttcimproverdates[i].title + '</a></div></div></div></div><div class="dashboard-list-item-right table-cell text-right" style="width: 20%><div class="gray-medium clearfix">' + ttcimproverdates[i].num + ' ' + ttcimproverdates[i].unit + ' ago</div></div></div>');
            }
        });
    }
    function buildfavsdash() {
        if (localStorage.getItem("ttcifavorites") === null) {
            var toptranslators;
            if(ttcilanguage == "Afrikaans") {toptranslators = ["GalauBang3t","haserkopf"];}
            else if(ttcilanguage == "Albanian") {toptranslators = ["bujartafili","genc_ks"];}
            else if(ttcilanguage == "Arabic") {toptranslators = ["MustafaFaour"];}
            else if(ttcilanguage == "Basque") {toptranslators = ["supro","Ieribi"];}              
            else if(ttcilanguage == "Belarusian") {toptranslators = ["Juschtell","00ZE"];}
            else if(ttcilanguage == "Bengali") {toptranslators = ["UdiptoRoy","gargi_hazra"];}
            else if(ttcilanguage == "Bulgarian") {toptranslators = ["peter_sl"];}
            else if(ttcilanguage == "Catalan") {toptranslators = ["no_hi_soc_tot"];}
            else if(ttcilanguage == "Croatian") {toptranslators = ["Peter_Dinu_","danasamaloglup"];}
            else if(ttcilanguage == "Czech") {toptranslators = ["verzus","themarketka"];}
            else if(ttcilanguage == "Danish") {toptranslators = ["helleras"];}
            else if(ttcilanguage == "Dutch") {toptranslators = ["LeonWetzel"];}
            else if(ttcilanguage == "english-uk") {toptranslators = ["Farhan_Danish","conradoldcorn"];}  
            else if(ttcilanguage == "Esperanto") {toptranslators = ["danieldaruma"];}
            else if(ttcilanguage == "Farsi") {toptranslators = ["Hassanvand","Kavelicious"];}
            else if(ttcilanguage == "Filipino") {toptranslators = ["micahsantos"];}
            else if(ttcilanguage == "Finnish") {toptranslators = ["LJuutilainen"];}
            else if(ttcilanguage == "French") {toptranslators = ["Orliox"];}
            else if(ttcilanguage == "Galician") {toptranslators = ["markooss"];}
            else if(ttcilanguage == "German") {toptranslators = ["das_ailton"];}
            else if(ttcilanguage == "Greek") {toptranslators = ["Lostologos"];}
            else if(ttcilanguage == "Hebrew") {toptranslators = ["Wallflower_r"];}
            else if(ttcilanguage == "Hindi") {toptranslators = ["Navrooz","alolita"];}
            else if(ttcilanguage == "Hungarian") {toptranslators = ["kkemenczy"];}
            else if(ttcilanguage == "Indonesian") {toptranslators = ["myifn"];}
            else if(ttcilanguage == "Italian") {toptranslators = ["insideRobb","laupezza"];}
            else if(ttcilanguage == "Japanese") {toptranslators = ["richardx64","haru703"];}
            else if(ttcilanguage == "Kannada") {toptranslators = ["Manjunatha_MN"];} // 1 missing
            else if(ttcilanguage == "Korean") {toptranslators = ["sapphire_dev","nrjeon"];}
            else if(ttcilanguage == "kurdish-central") {toptranslators = ["jwtiyar"];} // 1 missing
            else if(ttcilanguage == "kurdish-northern") {toptranslators = ["mryildiz7272"];} // 1 missing
            else if(ttcilanguage == "Latin") {toptranslators = ["LatinTranslate_"];} // 1 missing
            else if(ttcilanguage == "Latvian") {toptranslators = ["knifeless333","edzuslv"];}
            else if(ttcilanguage == "Malay") {toptranslators = ["aztazhr"];}
            else if(ttcilanguage == "Norwegian") {toptranslators = ["RockThatClock"];}
            else if(ttcilanguage == "Polish") {toptranslators = ["sylwiabesz"];}
            else if(ttcilanguage == "portuguese-brazil") {toptranslators = ["sukigu_","OlaKiridinha"];}
            else if(ttcilanguage == "Romanian") {toptranslators = ["TheGelu","oviung"];}
            else if(ttcilanguage == "Russian") {toptranslators = ["AlexAdvert","sprigoda"];}
            else if(ttcilanguage == "Serbian") {toptranslators = ["acko_aa"];}
            else if(ttcilanguage == "simplified-chinese") {toptranslators = ["ifansonia","sunnyjiangsj"];}
            else if(ttcilanguage == "Slovak") {toptranslators = ["matiqos"];}
            else if(ttcilanguage == "Spanish") {toptranslators = ["mathiascupito","monica"];}
            else if(ttcilanguage == "Swedish") {toptranslators = ["nedemekpembe"];}
            else if(ttcilanguage == "Tamil") {toptranslators = ["vijayasankar91","hari_vel"];}
            else if(ttcilanguage == "Thai") {toptranslators = ["nutmos"];}
            else if(ttcilanguage == "traditional-chinese") {toptranslators = ["ashaneba","candacec"];}
            else if(ttcilanguage == "Turkish") {toptranslators = ["aonurpkr","unbirthdaytea"];}
            else if(ttcilanguage == "Ukrainian") {toptranslators = ["SergiyAquila","MiraVognyu"];}
            else if(ttcilanguage == "Urdu") {toptranslators = ["FarhanDanish"];}
            else if(ttcilanguage == "Vietnamese") {toptranslators = ["Dikhotheta","Manoki_Lin"];}
            else if(ttcilanguage == "Welsh") {toptranslators = ["DewiEirig","aledpowell"];}
            else if(ttcilanguage == "Yoruba") {toptranslators = ["SamOlusegun"];} // 1 missing
            else {toptranslators = false;} // Burmese2, Irish1
        }
        if(((ttcilanguage != "Burmese") || (ttcilanguage != "Irish"))&& (localStorage.getItem("ttcifavorites") == null)) {
            localStorage.setItem('ttcifavorites', toptranslators);
        }
        if(localStorage.getItem("ttcifavorites")) {
            toptranslators = localStorage.getItem("ttcifavorites").split(',');
        }
        if(toptranslators !== false) {
            for (i = 0; i < toptranslators.length; i++) {
                if(toptranslators[i] != "none") {
                    lastprofileac(toptranslators[i], ".favoritesdash");
                }
            }
        }
        else {
            jqttci(".favoritesdash").append('<div class="dashboard-center-list-item table-row"><div class="dashboard-list-item-left adjacent-to-avatar table-cell" style="width: 100%"><div class="dashboard-user-info clearfix">Hey, what about tell us some top translators\' username?</div><div class="dashboard-activity-info clearfix"><a href="https://twitter.com/intent/tweet?screen_name=insideRobb&amp;text=I%20would%20suggest%20%40username1%20%26%20%40username2%20for%20TTC%20Improver" class="twitter-mention-button" data-size="large" data-related="insideRobb,TTC_Feed">Tweet to @insideRobb</a></div></div><div class="dashboard-list-item-right table-cell text-right gray-medium"></div></div>');
        }
    }
 
    var booleanize = function (value) {
        return (value != "false" && value != "" && value != "undefined" && value != "null" && value != "0");
    };
    var flex_encode = function (st) {
        return st.replace(/./g, function (s) {
            return "_" + s.charCodeAt(0);
        });
    };
    // Disable TTC Improver
    var currentusername = jqttci('.user-data').attr('data-user-login');
    var ttcimproverdisabled = localStorage.getItem("disablettcimprover");
    if (/^\/user\// + currentusername + /\/settings/.test(location.pathname)) {
        var ttcichecked;
        if (ttcimproverdisabled == "true") {
            ttcichecked = " checked";
        } else {
            ttcichecked = "";
        }
        jqttci('.edit_user').before('<fieldset class="control-group"><h3 class="settings-header" style="display: inline-block">TTC Improver</h3></fieldset><fieldset class="control-group"><div class="controls"><div class="control-list"><label class="checkbox"><input id="improver_disabled" type="checkbox"' + ttcichecked + '>Deactivate</label><label class="control-label" style="font-weight: normal">Developed by <a href="https://twitter.com/insideRobb" class="alert-link">@insideRobb</a>; projects icons by <a href="https://twitter.com/davideroliti" class="alert-link">@davideroliti</a><br><a href="http://robb.be/download/?forum=report-bug">Report bug</a> or <a href="http://robb.be/download/?forum=suggest-idea">send feedbacks</a> - <a href="http://robb.be/download/?page_id=14">Changelog</a></label></div></div></fieldset>');
        jqttci('#improver_disabled').click(function () {
            if (jqttci(this).is(':checked')) {
                localStorage.setItem("disablettcimprover", true);
            } else {
                localStorage.removeItem("disablettcimprover");
            }
        });
    }
 
    if(ttcimproverdisabled != "true") {
        jqttci("body").append("<div id='improverdata' style='display: none'></div><style>@-moz-keyframes spinner-loader{0%{-moz-transform:rotate(0);transform:rotate(0)}100%{-moz-transform:rotate(360deg);transform:rotate(360deg)}}@-webkit-keyframes spinner-loader{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);transform:rotate(360deg)}}@keyframes spinner-loader{0%{-moz-transform:rotate(0);-ms-transform:rotate(0);-webkit-transform:rotate(0);transform:rotate(0)}100%{-moz-transform:rotate(360deg);-ms-transform:rotate(360deg);-webkit-transform:rotate(360deg);transform:rotate(360deg)}}.spinner-loader:not(:required){-moz-animation:spinner-loader 1500ms infinite linear;-webkit-animation:spinner-loader 1500ms infinite linear;animation:spinner-loader 1500ms infinite linear;-moz-border-radius:.5em;-webkit-border-radius:.5em;border-radius:.5em;-moz-box-shadow:rgba(0,0,51,.3)1.5em 0 0 0,rgba(0,0,51,.3)1.1em 1.1em 0 0,rgba(0,0,51,.3)0 1.5em 0 0,rgba(0,0,51,.3)-1.1em 1.1em 0 0,rgba(0,0,51,.3)-1.5em 0 0 0,rgba(0,0,51,.3)-1.1em -1.1em 0 0,rgba(0,0,51,.3)0 -1.5em 0 0,rgba(0,0,51,.3)1.1em -1.1em 0 0;-webkit-box-shadow:rgba(0,0,51,.3)1.5em 0 0 0,rgba(0,0,51,.3)1.1em 1.1em 0 0,rgba(0,0,51,.3)0 1.5em 0 0,rgba(0,0,51,.3)-1.1em 1.1em 0 0,rgba(0,0,51,.3)-1.5em 0 0 0,rgba(0,0,51,.3)-1.1em -1.1em 0 0,rgba(0,0,51,.3)0 -1.5em 0 0,rgba(0,0,51,.3)1.1em -1.1em 0 0;box-shadow:rgba(0,0,51,.3)1.5em 0 0 0,rgba(0,0,51,.3)1.1em 1.1em 0 0,rgba(0,0,51,.3)0 1.5em 0 0,rgba(0,0,51,.3)-1.1em 1.1em 0 0,rgba(0,0,51,.3)-1.5em 0 0 0,rgba(0,0,51,.3)-1.1em -1.1em 0 0,rgba(0,0,51,.3)0 -1.5em 0 0,rgba(0,0,51,.3)1.1em -1.1em 0 0;display:inline-block;font-size:10px;width:1em;height:1em;margin:1.5em;overflow:hidden;text-indent:100%;margin-right: auto; font-size: 6px; display: block; margin-left: auto; position: relative; top: -6px;}</style>");
        var i;  
 
        //Detect if user is a moderator, vendor or admin
        var ttcstaff = false;
        if (jqttci(".navbar-right a[href='/admin']").length > 0) {
            ttcstaff = true;
        }
 
        //Warning for unnecessary symbols at the end
        if (jqttci("#search-form").length == 1) {
            if ((window.location.href.indexOf("https://translate.twitter.com/phrases/") != -1)||(jqttci("#search-form").length == 1)) {
                jqttci("#q").addClass("form-control");
                jqttci("#q").css("margin-top", "7px").css("width", "90%");
                jqttci("#phrase-search-submit").css("height", "30px").css("padding-top", "4px").css("margin-top", "0px");
                jqttci("#search-form .input-group").prepend('<div class="modal fade" id="searchOperators" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"><div class="modal-dialog" role="document"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title" id="myModalLabel">Search Operators</h4></div><div class="modal-body" style="font-size: 14px; color: #555"><ul><li><b>intranslation: </b>Search for terms in translations instead of source content</li><li><b>tag: </b>Search for phrases tagged with the specified tag</li><li><b>notag: </b>Search for phrases tagged without any tag</li><li><b>url: </b>Search for phrases that have a URL that contains the term specified</li><li><b>nourl: </b>Search for phrases without any URL</li><li><b>comment: </b>Search for phrases that have a note that contains the term specified</li><li><b>nocomment: </b>Search for phrases without any note</li><li><b>meta_key: </b>Search for phrases by meta key</li><li><b>global: </b>Search across all projects</li><li><b>verbatim: </b>Search for exact matches</li></ul><a href="/forum/forums/useful-guidelines/topics/7675">Learn more</a></div></div></div></div><button type="button" class="btn btn-sm btn-info" data-toggle="modal" data-target="#searchOperators" style="margin-right: 5px"><i class="glyphicon glyphicon-info-sign"></i></button><div style="margin-right: 5px" class="btn-group"><button type="button" class="btn btn-default btn-sm dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Operators <span class="caret"></span></button><ul class="dropdown-menu"><li><a href="#">intranslation:</a></li><li><a href="#">tag:</a></li><li><a href="#">notag:</a></li><li><a href="#">url:</a></li><li><a href="#">nourl:</a></li><li><a href="#">comment:</a></li><li><a href="#">nocoment:</a></li><li><a href="#">meta_key:</a></li><li><a href="#">global:</a></li><li><a href="#">verbatim:</a></li><li><a href="#">global:verbatim:</a></li></ul></div>');
                jqttci("#search-form ul a").click(function() {
                    jqttci("#q").focus().val(jqttci(this).text());
                });
                if (jqttci(".full-height .alert-info").length != 1) {
                    ttciinterval('1');
 
                    var target = jqttci("#translations .translations-table > tbody")[0];
                    var observer = new MutationObserver(function( mutations ) {
                        ttciinterval('6');
                    });
                    var obconfig = {childList: true};
                    observer.observe(target, obconfig);
 
 
                    jqttci("ul#phrases").on("click","li", function(){
                        ttciinterval('1');
                    });
                }
            }
        }
        //Forum buttons
        if(jqttci("#new_topic").length||jqttci("#new_post").length||jqttci(".edit_post").length) {
            jqttci("textarea.text").before('<style>.body-forem-posts .container > h2, .body-forem-posts .topic_subject {display: inline-block} .body-forem-posts .topic_subject:before {content: open-quote} .body-forem-posts .topic_subject:after {content: close-quote}</style><div class="btn-group formatter" role="group"><a href="#" class="btn btn-default" style="font-style: italic">i</a><a class="btn btn-default" style="font-weight: bold">b</a><a class="btn btn-default">Link</a><a class="btn btn-default">List item</a><div class="btn-group" role="group"><button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Header<span class="caret"></span></button><ul class="dropdown-menu"><li><a href="#" title="#">H1</a></li><li><a href="#" title="##">H2</a></li><li><a href="#" title="###">H3</a></li><li><a href="#" title="####">H4</a></li><li><a href="#" title="#####">H5</a></li><li><a href="#" title="######">H6</a></li></ul></div></div><br><br>');      
            jqttci(".formatter > a:first-child").click(function() {
                forumdashboard("*");
            });
            jqttci(".formatter > a:nth-child(2)").click(function() {
                forumdashboard("**");
            });
            jqttci(".formatter > a:nth-child(3)").click(function() {
                forumdashboard("link");
            });
            jqttci(".formatter > a:nth-child(4)").click(function() {
                forumdashboard("* ");
            });
            jqttci(".formatter .dropdown-menu a").click(function() {
                forumdashboard(this.title);
            });
        }
 
        //Icons
 
        if((jqttci(".clearfix .pull-right a[href='/translate']").length > 0)||(document.URL == "https://translate.twitter.com/translate")) {  
            var icons = [];
            icons.push(["about-twitter", "https://pbs.twimg.com/media/BoG1a6LCIAA5Cb-.png"]);
            icons.push(["amplify-player", "https://pbs.twimg.com/media/BqphXRdIgAEptHd.png"]);
            icons.push(["cards", "https://pbs.twimg.com/media/BoG0lHfCIAAqIRG.png"]);
            icons.push(["emails", "https://pbs.twimg.com/media/BoG0lHTCEAEzcRl.png"]);
            icons.push(["microsites", "https://pbs.twimg.com/media/BoG0lHgCAAAT9hr.png"]);
            icons.push(["m2", "https://pbs.twimg.com/media/B_qaW0GXAAAKgCM.png"]);
            icons.push(["responsive-web", "https://pbs.twimg.com/media/BoG0yF1CIAA9DjY.png"]);
            icons.push(["sms", "https://pbs.twimg.com/media/BoG0yHtCUAAOEUE.png"]);
            icons.push(["periscope-android", "https://pbs.twimg.com/media/CH5NC8MUwAA7FqH.png"]);
            icons.push(["periscope-ios", "https://pbs.twimg.com/media/CH5NC8OUcAA6r3T.png"]);
            icons.push(["periscope-web", "https://pbs.twimg.com/media/CH5NC8QUkAAi31H.png"]);
            icons.push(["platform", "https://pbs.twimg.com/media/BoG0yIACUAA4IGU.png"]);
            icons.push(["support-twitter", "https://pbs.twimg.com/media/BoG1gQuCcAAku84.png"]);
            icons.push(["transparency-twitter", "https://pbs.twimg.com/media/B_qbejqWYAADSIo.png"]);
            icons.push(["tweetdeck", "https://pbs.twimg.com/media/BoG1a5yCAAA9Kag.png"]);
            icons.push(["twitter-dev-android", "https://pbs.twimg.com/media/B7JjGVqIEAAS5yD.png"]);
            icons.push(["twitter-dev-ios", "https://pbs.twimg.com/media/B7pGLyVCQAA6Q50.png"]);
            icons.push(["twitter-dev-web", "https://pbs.twimg.com/media/B_qbemAWcAAMmw8.png"]);
            icons.push(["android", "https://pbs.twimg.com/media/BoG1a6tCIAAv--A.png"]);
            icons.push(["twitter-iphone", "https://pbs.twimg.com/media/BoG1a6fCMAAWti3.png"]);
            icons.push(["twitter-mac", "https://pbs.twimg.com/media/B_qaWz6WwAAWBNC.png"]);
            icons.push(["twitter-windowsphone", "https://pbs.twimg.com/media/BoG1gPRCQAAr0yP.png"]);
            icons.push(["twitter-mirror-ios", "https://pbs.twimg.com/media/BoG1lRNCEAAb30K.png"]);
            icons.push(["twitter-music", "https://pbs.twimg.com/media/BoG1lVLCIAAvPpq.png"]);
            icons.push(["twitter", "https://pbs.twimg.com/media/BoG1lWuCcAAeX0B.png"]);
            icons.push(["VineAPI", "https://pbs.twimg.com/media/BoG1lW_CMAAsyu3.png"]);
            icons.push(["vine-android", "https://pbs.twimg.com/media/BoG1si6CYAAd_mh.png"]);
            icons.push(["vine-ios", "https://pbs.twimg.com/media/BoG1ssCCEAAn-Jh.png"]);
            icons.push(["vine-windowsphone", "https://pbs.twimg.com/media/BoG1ssiCQAE9MZ8.png"]);
            icons.push(["glossary_project", "https://pbs.twimg.com/media/BvUJyqsCMAAer5k.png"]);
            icons.push(["twitter-ads", "https://pbs.twimg.com/media/BoPW3gyIUAAxJ--.png"]);
            icons.push(["dev-twitter", "https://pbs.twimg.com/media/B_qbemuWEAAZIk_.png"]);
            icons.push(["jobs", "https://pbs.twimg.com/media/BoG1gRaCQAAPzMo.png"]);
            icons.push(["glossary_project_test_onl", "https://pbs.twimg.com/media/BvUJyqsCMAAer5k.png"]);
 
            if (document.URL == "https://translate.twitter.com/translate") {
                jqttci('head').append('<style type="text/css">.dashboard-center .table a img {background-image: none}</style>');
                for(i = 0; i < icons.length; i++) {
                    jqttci('.dashboard-center .table a[href="/' + icons[i][0] + '"] img').prop('src', icons[i][1]);
                }
                jqttci("#improverdata").load("/user/" + currentusername + " .stats div.stat > .stat-value", function() {
                    var helldashdiv = document.querySelectorAll(".dashboard-sidebar .dashboard-sidebar-divider")[1];
                    helldashdiv.insertAdjacentHTML('beforebegin', '<div class="height10"></div><div class="clearfix"><strong class="stat-value">' + jqttci("#improverdata").text().replace(',','') + '</strong><div class="stat-name text-muted">KARMA POINTS</div></div>');
                });
            }
            else {
                jqttci('head').append('<style type="text/css">#projectsDropdownMenu .pull-left img {background-image: none}</style>');
                for(i = 0; i < icons.length; i++) {
                    if (document.URL == "https://translate.twitter.com/" + icons[i][0]) {
                        jqttci('#projectsDropdownMenu .pull-left img').prop('src', icons[i][1]);
                        i = icons.length;
                    }
                }
            }
        }
 
        //Profiles
        if ((window.location.href.indexOf("https://translate.twitter.com/user/") != -1) && (jqttci(".profile-name").length) && (jqttci("#oauth-connect").length === 0)) {
            jqttci('.stats a[href$="votes_for"] .stat-value').html(jqttci('.stats a[href$="votes_for"] .stat-value').text().replace(',',''));
            jqttci('.stats > div .stat-value').html(jqttci('.stats > div .stat-value').text().replace(',',''));
            var getusername = jqttci('.profile-user').attr('data-user-login');
            jqttci('.stats a:not(a[href$="/votes_for"]) .stat-value').text('Loading');
            cache(getusername);
            if (cache(getusername) === false) {
                ttciicount("translations", "1", "translations");
                ttciicount("translations/approved", "2", "translations_approved");
                ttciicount("votes", "3", "votes");
                ttciicount("votes/approved", "4", "votes_approved");
                sessionStorage.setItem('cache' + getusername, true);
 
            } else {
                jqttci('.stats a:nth-of-type(1) .stat-value').text(sessionStorage.getItem('translations_' + getusername));
                jqttci('.stats a:nth-of-type(2) .stat-value').text(sessionStorage.getItem('translations_approved_' + getusername));
                jqttci('.stats a:nth-of-type(3) .stat-value').text(sessionStorage.getItem('votes_' + getusername));
                jqttci('.stats a:nth-of-type(4) .stat-value').text(sessionStorage.getItem('votes_approved_' + getusername));
            }
 
            if ((jqttci(".user-achievements").length == 0) && (jqttci(".profile-name").length)) {
                jqttci(".stats").after('<div class="user-achievements"><div class="clearfix"><h2 class="pull-left">Achievements</h2></div><div class="clearfix achievement-group translator-level"><p class="muted achievement-group-name">Translator Level</p><div class="achievement clearfix"><img alt="achievements_soaring_large" class="pull-left achievement-image achievements_soaring_large" src=""><div class="achievement-info clearfix pull-left"><div class="clearfix"><span class="achievement-name"></span></div><div class="achievement-description muted clearfix"></div></div></div></div></div>');
                var levelbadge = jqttci(".translator-level .achievement > img");
                var levelname = jqttci(".translator-level .achievement-name");
                var leveldescription = jqttci(".translator-level .achievement-description");
                var approved = jqttci('.stats a[href$="translations/approved"] .stat-value').text();
 
                if (/LbTransOnly$/i.test(getusername) || /LB$/.test(getusername) || /QALiox$/i.test(getusername) || /LQA$/i.test(getusername) || /(ENGBQALiox1|PortugueseQA|GermanSMB1|GermanSMB2|GermanSMB3|DADK2LbTransOnl|DADK3LbTransOnl|DADK4LbTransOnl|UkrainianTr1|UkrainianTr2|UkrainianTr3|UkrainianTr4|SvLbTransOnly|PTBR_Translator|LBPolish2|JapaneseLBProd)/.test(getusername)) {
                    levelbadge.attr("src", "https://pbs.twimg.com/media/BvUK-7nCQAAp44F.png");
                    levelname.text("Vendor");
                    leveldescription.html("Professional translator employed by Twitter; <a href='https://translate.twitter.com/forum/forums/indonesian/topics/6269#post-39296'>learn more</a>");
                } else if (/(conradoldcorn|Kavelicious|unbirthdaytea|b_eimon|monica|bigloser|jakl|nrjeon|DayvieO|haru703|OkidoKim|winfield|constantly|tarmstrong|NLPenguin|edeng|twitter_kr|candacec|MelikeSF|KL7|JasonYZhao|laupezza|imjohnwalsh|sprigoda|alolita|tm|gargi_hazra|FrancescaDM|gianna|jiangts|sunnyjiangsj|mrkyten)/.test(getusername)) {
                    levelbadge.attr("src", "https://pbs.twimg.com/media/BvhcfnZIcAQQsOQ.png");
                    levelname.text("Localizer");
                    leveldescription.html("Member of Twitter <a href='https://twitter.com/localizers'>Localization</a> Team");
                }
            }
        }
 
        if(jqttci(".body-dashboard-homepage").length) {
            jqttci("head").append('<style type="text/css">.dashboard-center-list-header {font-size: 24px; line-height: 19px} .dashboar-user-rank .volunteer-since {font-size: 13px} .dashboard-list-item-left.adjacent-to-avatar {width: auto} .dashboard-activity-list .adjacent-to-avatar {width: 70%}</style>');
 
            cache('feedback');
            if (cache('feedback') === false) {
                jqttci("#improverdata").load("/learn .dashboard-center > div:nth-child(3) h3", function() {
                    if(jqttci("#improverdata h3:first-child").text() == "Feedback") {
                        var ttcifeedback = jqttci("#improverdata h3:last-child").text();
                        sessionStorage.setItem("ttcifeedback", ttcifeedback);
                        if(sessionStorage.getItem('ttcifeedback') > 0) {document.querySelector('.dashboard-headers .dashboard-header:nth-child(3) .dashboard-header-title').innerHTML+=" <span class='badge badge-info'>" + sessionStorage.getItem('ttcifeedback') + "</span>";}
                    }
                });
            }
            else {
                if(sessionStorage.getItem('ttcifeedback') > 0) {document.querySelector('.dashboard-headers .dashboard-header:nth-child(3) .dashboard-header-title').innerHTML+=" <span class='badge badge-info'>" + sessionStorage.getItem('ttcifeedback') + "</span>";}
            }
            jqttci(".dashboar-user-rank").after('<br><div class="clearfix"><h4 class="dashboard-moderators-title clearfix" style="font-size: 12pt">Get updates about new strings</h4><div class="dashboard-moderator clearfix"><a href="/user/TTC_Feed"><div class="dashboard-small-image clearfix pull-left"><img alt="TTC Feed" class="pull-left profile-image img-rounded" src="https://pbs.twimg.com/profile_images/426006182772236288/Op44ZXb4_reasonably_small.png"></div></a><div class="dashboard-moderator-info clearfix pull-left"><a href="/user/TTC_Feed"><div class="dashboard-moderator-name black">TTC Feed</div></a><a href="https://twitter.com/TTC_Feed"><div class="gray-light">@TTC_Feed</div></a></div><div class="dashboard-moderator-follow pull-right"><iframe id="twitter-widget-4" scrolling="no" frameborder="0" allowtransparency="true" src="https://platform.twitter.com/widgets/follow_button.1400006231.html#_=1400623989072&amp;id=twitter-widget-4&amp;lang=en&amp;screen_name=TTC_Feed&amp;show_count=false&amp;show_screen_name=false&amp;size=m" class="twitter-follow-button twitter-follow-button" title="Twitter Follow Button" data-twttr-rendered="true" style="width: 60px; height: 20px;"></iframe></div></div></div>');
 
            //Forum widget
            var ttcilanguage = document.querySelector(".dashboard-hashtag .gray-dark").textContent.split(' ')[0];
            if(ttcilanguage == "English (UK)") {ttcilanguage = "english-uk";}
            else if(ttcilanguage == "Kurdish (Central)") {ttcilanguage = "kurdish-central";}
            else if(ttcilanguage == "Kurdish (Northern)") {ttcilanguage = "kurdish-northern";}
            else if(ttcilanguage == "Portuguese (Brazil)") {ttcilanguage = "portuguese-brazil";}
            else if(ttcilanguage == "Simplified Chinese") {ttcilanguage = "simplified-chinese";}
            else if(ttcilanguage == "Traditional Chinese") {ttcilanguage = "traditional-chinese";}
            jqttci(".dashboard-sidebar").after('<div class="dashboard-center clearfix pull-right"><div class="dashboard-center-list dashboard-latest-posts clearfix"><h3 class="dashboard-center-list-header clearfix">Latest posts in <a href="/forum/forums/' + ttcilanguage + '">' + ttcilanguage + ' forum</a></h3><div class="dashboard-center-list-body table-layout clearfix forumdash" style="display: block;"><div class="spinner-loader">Loading…</div></div></div></div>');
            if (localStorage.getItem("TranslateTwitterAdapt_dashboardstate" + flex_encode(jqttci(".dashboard-latest-posts").attr("class"))) != "false") {
                buildforumdash();
            }
            else {
                var c = 0;
                jqttci(document).ready(function() {
                    jqttci(".dashboard-latest-posts .close").click(function() {
                        if(c == 0) {
                            buildforumdash();
                            c++;
                        }
                    });                  
                });
            }
 
            //Beta approvers
            if((ttcilanguage == "Irish")||(ttcilanguage == "Esperanto")||(ttcilanguage == "Galician")) {
                var locale = {
                    code: '',
                    approvers: []
                };
                if(ttcilanguage == "Irish") {locale.code = "GA"; locale.approvers = [["bhriain","Brian","460092892149592064/VP1v-DjZ_reasonably_small.jpeg"],["IndigenousTweet","Indigenous Tweets","3379056719/aaae80cfbb47a92d9df7bc5cc1058b64_reasonably_small.jpeg"],["Marcas_OLoinn","Marcas ÓLoinneacháin","1700212843/funny_bird_reasonably_small.jpg"],["murchadhmor","Murchadh Mór","642288423752105984/0axAiLCA.jpg"],["breathnachc","Cormac Breathnach","441165336474165248/DwcytK8v_reasonably_small.jpeg"],["MOMeachair","MícheálJohnny","634733708981964800/gpX0STYY_reasonably_small.jpg"]];}
                else if(ttcilanguage == "Esperanto") {locale.code = "EO"; locale.approvers = [["Vikinovajhoj","Vikinovaĵoj","378800000084719631/22da1517c82a3d320ff261dd17b2a700_reasonably_small.png"],["tcql","Tim Channell","447555545122082816/Yg-QJzKc_reasonably_small.png"]];}
                else if(ttcilanguage == "Galician") {locale.code = "GL"; locale.approvers = [["braisvarela23","Brais Varela","2138821596/4KK895G2_reasonably_small"],["migguis","~","378800000118289814/3f85f2965f451773a1ff045bbbc03de1_reasonably_small.gif"]];}
                jqttci(".dashboard-hashtag-search").after('<div class="dashboard-moderators dashboard-approvers clearfix"><h4 class="dashboard-moderators-title clearfix">#TTC_' + locale.code + ' approvers</h4></div>');
                for (i = 0; i < locale.approvers.length; i++) {
                    jqttci(".dashboard-approvers h4").after('<div class="dashboard-moderator clearfix"><a href="/user/' + locale.approvers[i][0] + '"><div class="dashboard-small-image clearfix pull-left"><img alt="' + locale.approvers[i][1] + '" class="pull-left profile-image img-rounded" src="//pbs.twimg.com/profile_images/' + locale.approvers[i][2] + '"></div></a><div class="dashboard-moderator-info clearfix pull-left"><a href="/user/' + locale.approvers[i][0] + '"><div class="dashboard-moderator-name black">' + locale.approvers[i][1] + '</div></a><a href="https://twitter.com/' + locale.approvers[i][0] + '"><div class="dashboard-moderator-username gray-light">@' + locale.approvers[i][0] + '</div></a></div><div class="dashboard-moderator-follow pull-right"><iframe id="twitter-widget-0" scrolling="no" frameborder="0" allowtransparency="true" src="https://platform.twitter.com/widgets/follow_button.331904cc91ddebde387d36578bfb9deb.en.html#_=1421631403208&amp;dnt=false&amp;id=twitter-widget-0&amp;lang=en&amp;screen_name=' + locale.approvers[i][0] + '&amp;show_count=false&amp;show_screen_name=false&amp;size=m" class="twitter-follow-button twitter-follow-button" title="Twitter Follow Button" data-twttr-rendered="true" style="width: 59px; height: 20px;"></iframe></div></div>');
                }
            }
 
            //Mods activity
            if(jqttci(".dashboard-moderators").length) {
                jqttci(".dashboard-sidebar").after('<div class="dashboard-center clearfix pull-right"><div class="dashboard-center-list dashboard-activity-moderators"><h3 class="dashboard-center-list-header clearfix">Moderators activity</h3><div class="dashboard-center-list-body table-layout moderatorsdash"><div class="spinner-loader">Loading…</div></div></div></div>');
                if (localStorage.getItem("TranslateTwitterAdapt_dashboardstate" + flex_encode(jqttci(".dashboard-activity-moderators").attr("class"))) != "false") {
                    buildmodsdash();
                }
                else {
                    var c = 0;
                    jqttci(document).ready(function() {
                        jqttci(".dashboard-activity-moderators .close").click(function() {
                            if(c == 0) {
                                buildmodsdash();
                                c++;
                            }
                        });                  
                    });
                }
            }
 
            // Favorites activity
 
            if(ttcilanguage != "Lolcat") {
                jqttci(".dashboard-center.pull-right:nth-last-child(3)").before('<div class="dashboard-center clearfix pull-right"><div class="dashboard-center-list dashboard-activity-favorites"><h3 class="dashboard-center-list-header clearfix"><a href="#" id="addfavorite">+</a> Favorites activity <small>(<a href="#" class="favoritedelete">clear</a>)</small></h3><div class="dashboard-center-list-body table-layout favoritesdash" style="display: block;"><div class="spinner-loader">Loading…</div></div></div></div>');
                if (localStorage.getItem("TranslateTwitterAdapt_dashboardstate" + flex_encode(jqttci(".dashboard-activity-favorites").attr("class"))) != "false") {
                    buildfavsdash();
                }
                else {
                    var c = 0;
                    jqttci(document).ready(function() {
                        jqttci(".dashboard-activity-favorites .close").click(function() {
                            if(c == 0) {
                                buildfavsdash();
                                c++;
                            }
                        });                  
                    });
                }
                jqttci("#addfavorite").click(function() {
                    if (localStorage.getItem("TranslateTwitterAdapt_dashboardstate" + flex_encode(jqttci(".dashboard-activity-favorites").attr("class"))) == "false") {
                        jqttci(".dashboard-activity-favorites .close").click();
                    }
                    var newfavorite = prompt("Please enter a username to add to this widget", "favoriteusername");
                    if(newfavorite !== null) {
                        toptranslators[toptranslators.length] = newfavorite;
                        localStorage.setItem('ttcifavorites', toptranslators);
                        lastprofileac(newfavorite, ".favoritesdash");
                    }
                });
                jqttci(".favoritedelete").click(function() {
                    localStorage.removeItem("ttcifavorites");
                    toptranslators = [];
                    jqttci(".favoritesdash .dashboard-center-list-item").remove();
                });
            }
 
            //Dashboard by Flexlex
            var dashboard = function (p) {
                var dashboardstate = true;
                var id = flex_encode(p.className);
                var dash = {
                    dashboard: true,
                    container: p,
                    header: p.querySelector("h3.dashboard-center-list-header"),
                    button: null,
                    body: p.querySelector("div.dashboard-center-list-body"),
                    set state(value) {
                        var bool = booleanize(value);
                        dashboardstate = bool;
                        localStorage["TranslateTwitterAdapt_dashboardstate" + id] = bool;
                        dash.body.style.display = bool ? "block" : "none";
                        if (dash.button)
                            dash.button.innerHTML = bool ? "&#215;" : "&#9662;";
                    },
                    get state() {
                        return booleanize(localStorage["TranslateTwitterAdapt_dashboardstate" + id]);
                    }
                };
                dash.state = (booleanize(localStorage) && booleanize(localStorage["TranslateTwitterAdapt_dashboardstate" + id]));
                dash.header.insertAdjacentHTML("beforeEnd", "<button class=\"close\">" + (dash.state ? "&#215;" : "&#9662;") + "</button>");
                dash.button = dash.header.querySelector("button");
                dash.button.addEventListener("click", function () {
                    dash.state = !dash.state;
                }, false);
                dash.button.style.cursor = "pointer";
            };
            var dashboards_match = document.querySelectorAll("div.dashboard-center-list");
            var dashboards = [];
            for (i = dashboards_match.length; --i >= 0;) {
                dashboards.push(new dashboard(dashboards_match[i]));
            }
 
            this.dashboard = dashboards;
            this.bool = booleanize;
        }
 
        if ((window.location.href.indexOf("https://translate.twitter.com/learn") != -1)&&(jqttci('.dashboard-tile-image img[alt="Lessons"]').length)) {
            jqttci(".dashboard-center .dashboard-tile:first-child .dashboard-tile-right h3").text("Loading...");
            jqttci("#improverdata").load("/lessons .label", function() {
                var ttcitotallessons = jqttci("#improverdata .label").length;
                var ttcitotallessonsfinished = jqttci("#improverdata .complete").length;
                jqttci(".dashboard-center .dashboard-tile:first-child .dashboard-tile-right h3").text(ttcitotallessonsfinished + "/" + ttcitotallessons);
            });
        }
 
        //links dropdown
        jqttci(".nav-secondary.pull-right > li:nth-last-child(2) ul").prepend('<li><a href="/forum/forums/getting-started/topics/7042">How it works</a></li><li><a href="/forum/forums/useful-guidelines/topics/7039">General style guidelines</a></li>');
    }
    jqttci("#improverdata").html("");
}
TranslateTwitterAdapt();
