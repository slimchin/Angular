var unhidden = [];
var expandall_list = [];
var expandall_groups = [];
var expandall_indexes = [];

function cursor_wait() {
    document.body.style.cursor = 'wait';
}
function cursor_clear() {
    document.body.style.cursor = 'default';
}
function submitForm(id) {
    var validateForm = document.getElementById(id);
    validateForm.submit();
}
function ShowDiv(divname) {
    document.getElementById(divname).style.display = "";
}
function HideDiv(divname) {
    document.getElementById(divname).style.display = "none";
}
function HideOrShowDivUpdateIcon(divname) {
    if (document.getElementById(divname).style.display === 'none') {
        ShowDiv(divname);
        document.getElementById(divname + '_icon').src = document.getElementById('image_minus').src;
        document.getElementById(divname + '_icon').alt = 'Collapse';
        document.getElementById(divname + '_icon').title = 'Collapse';
    }
    else {
        HideDiv(divname);
        document.getElementById(divname + '_icon').src = document.getElementById('image_plus').src;
        document.getElementById(divname + '_icon').alt = 'Expand';
        document.getElementById(divname + '_icon').title = 'Expand';
    }
}
function isArray(a) {
    return isObject(a) && a.constructor == Array;
}
function isObject(a) {
    return (a && typeof a == 'object') || isFunction(a);
}
function isFunction(a) {
    return typeof a == 'function';
}
function isUndefined(a) {
    return typeof a == 'undefined';
}
function addExpandAllGroup(groupName) {
    var groupIDX = expandall_groups.length;

    expandall_groups[groupIDX] = groupName;
    if (!isArray(expandall_indexes[groupIDX])) {
        expandall_indexes[groupIDX] = 0;
    }
    if (!isArray(expandall_list[groupIDX])) {
        expandall_list[groupIDX] = [];
    }
    return groupIDX;
}
function getExpandAllGroupID(groupName) {
    var x;

    for (x = 0; x < expandall_groups.length; x++) {
        if (expandall_groups[x] == groupName) {
            return x;
        }
    }
    x = addExpandAllGroup(groupName);
    return x;
}
function updateExpandAllText() {
    var x;

    for (x = 0; x < expandall_groups.length; x++) {
        if (isArray(expandall_list[x])) {
            if (expandall_list[x].length > 0) {

                if (document.getElementById(expandall_groups[x] + '_display') != 'undefined' & document.getElementById(expandall_groups[x] + '_display') != null) {
                    document.getElementById(expandall_groups[x] + '_display').style.display = "";
                }
                if (document.getElementById(expandall_groups[x] + '_undisplay') != 'undefined' && document.getElementById(expandall_groups[x] + '_undisplay') != null) {
                    document.getElementById(expandall_groups[x] + '_undisplay').style.display = "none";
                }
            }
        }
    }
}
function addToExpandList(groupName, divName) {
    var groupIDX = getExpandAllGroupID(groupName);
    var i = expandall_list[groupIDX].length;

    expandall_list[groupIDX][i] = divName;
}
function ExpandAllInReport(groupName) {
    var groupIDX = getExpandAllGroupID(groupName);
    console.log(groupIDX);

    if (document.getElementById(groupName + '_icon').alt == 'Expand All') {
        document.getElementById(groupName + '_display_text').innerHTML = 'Collapse All';
        document.getElementById(groupName + '_icon').alt = 'Collapse All';
        document.getElementById(groupName + '_icon').title = 'Collapse All';
        document.getElementById(groupName + '_icon').src = document.getElementById('image_minus_all').src;

        for (var x = 0; x < expandall_list[groupIDX].length; x++) {
            document.getElementById(expandall_list[groupIDX][x] + '_icon').src = document.getElementById('image_minus').src;
            document.getElementById(expandall_list[groupIDX][x] + '_icon').alt = 'Collapse';
            document.getElementById(expandall_list[groupIDX][x] + '_icon').title = 'Collapse';
            ShowDiv(expandall_list[groupIDX][x]);
        }
    }
    else {
        document.getElementById(groupName + '_display_text').innerHTML = 'Expand All';
        document.getElementById(groupName + '_icon').alt = 'Expand All';
        document.getElementById(groupName + '_icon').title = 'Expand All';
        document.getElementById(groupName + '_icon').src = document.getElementById('image_plus_all').src;

        var x;

        for (x = 0; x < expandall_list[groupIDX].length; x++) {
            document.getElementById(expandall_list[groupIDX][x] + '_icon').src = document.getElementById('image_plus').src;
            document.getElementById(expandall_list[groupIDX][x] + '_icon').alt = 'Expand';
            document.getElementById(expandall_list[groupIDX][x] + '_icon').title = 'Expand';
            HideDiv(expandall_list[groupIDX][x]);
        }
    }
}

var unhiddenValidatePopup = 'new';
var popup_left = 5;
var popup_top = 10;

function ShowTraffic(guid, id) {
    framename = 'trafficframe_' + id;
    divname = 'trafficdiv_' + id;
    div_ref = document.getElementById(divname);
    if (document.getElementById(divname).style.display == 'none') {
        div_ref.innerHTML = '<iframe id="' + framename + '" src="traffic/t' + guid + '.html" width="100%" style="display:block;"><\/iframe>';
        HideOrShowDivUpdateIcon(divname);
        document.getElementById(framename).style.height = '400px';
        document.getElementById(framename).style.width = '100%';
        document.getElementById(framename).style.display = 'block';
    }
    else {
        HideOrShowDivUpdateIcon(divname);
        div_ref.innerHTML = '';
    }
}

function ShowValidatePopup(divname, divhandle) {
    if (unhiddenValidatePopup != 'new') {
        HideDiv(unhiddenValidatePopup);
    }
    unhiddenValidatePopup = divname;
    ShowDiv(divname);

    var theHandle = document.getElementById(divhandle);
    var theRoot = document.getElementById(divname);

    Drag.init(theHandle, theRoot);
}

var unhiddenLevel2 = 'new';

function ShowLevel2Div(divname, divhandle) {
    if (unhiddenLevel2 != 'new') {
        HideLevel2Div(unhiddenLevel2);
    }
    unhiddenLevel2 = divname;
    document.getElementById(divname).style.left = popup_left;
    document.getElementById(divname).style.top = popup_top;
    ShowDiv(divname);

    var theHandle = document.getElementById(divhandle);
    var theRoot = document.getElementById(divname);

    Drag.init(theHandle, theRoot);
}
function HideLevel2Div(divname) {
    if (unhiddenLevel3 != 'new') {
        HideDiv(unhiddenLevel3);
    }
    unhiddenLevel2 = 'new';
    popup_left = document.getElementById(divname).style.left;
    popup_top = document.getElementById(divname).style.top;
    HideDiv(divname);
}

var unhiddenLevel3 = 'new';

function ShowLevel3Div(divname) {
    if (unhiddenLevel3 != 'new') {
        HideDiv(unhiddenLevel3);
    }
    unhiddenLevel3 = divname;
    ShowDiv(divname);
}

var Drag =
    {
        obj: null,
        init: function (o, oRoot, minX, maxX, minY, maxY, bSwapHorzRef, bSwapVertRef, fXMapper, fYMapper) {
            o.onmousedown = Drag.start;
            o.hmode = !bSwapHorzRef;
            o.vmode = !bSwapVertRef;

            o.root = oRoot && oRoot != null ? oRoot : o;

            if (o.hmode && isNaN(parseInt(o.root.style.left))) o.root.style.left = "0px";
            if (o.vmode && isNaN(parseInt(o.root.style.top))) o.root.style.top = "0px";
            if (!o.hmode && isNaN(parseInt(o.root.style.right))) o.root.style.right = "0px";
            if (!o.vmode && isNaN(parseInt(o.root.style.bottom))) o.root.style.bottom = "0px";

            o.minX = typeof minX != 'undefined' ? minX : null;
            o.minY = typeof minY != 'undefined' ? minY : null;
            o.maxX = typeof maxX != 'undefined' ? maxX : null;
            o.maxY = typeof maxY != 'undefined' ? maxY : null;

            o.xMapper = fXMapper ? fXMapper : null;
            o.yMapper = fYMapper ? fYMapper : null;

            o.root.onDragStart = new Function();
            o.root.onDragEnd = new Function();
            o.root.onDrag = new Function();
        },
        start: function (e) {
            var o = Drag.obj = this;

            e = Drag.fixE(e);

            var y = parseInt(o.vmode ? o.root.style.top : o.root.style.bottom);
            var x = parseInt(o.hmode ? o.root.style.left : o.root.style.right);

            o.root.onDragStart(x, y);

            o.lastMouseX = e.clientX;
            o.lastMouseY = e.clientY;

            if (o.hmode) {
                if (o.minX != null) o.minMouseX = e.clientX - x + o.minX;
                if (o.maxX != null) o.maxMouseX = o.minMouseX + o.maxX - o.minX;
            }
            else {
                if (o.minX != null) o.maxMouseX = -o.minX + e.clientX + x;
                if (o.maxX != null) o.minMouseX = -o.maxX + e.clientX + x;
            }
            if (o.vmode) {
                if (o.minY != null) o.minMouseY = e.clientY - y + o.minY;
                if (o.maxY != null) o.maxMouseY = o.minMouseY + o.maxY - o.minY;
            }
            else {
                if (o.minY != null) o.maxMouseY = -o.minY + e.clientY + y;
                if (o.maxY != null) o.minMouseY = -o.maxY + e.clientY + y;
            }

            document.onmousemove = Drag.drag;
            document.onmouseup = Drag.end;

            return false;
        },
        drag: function (e) {
            e = Drag.fixE(e);

            var o = Drag.obj;
            var ey = e.clientY;
            var ex = e.clientX;
            var y = parseInt(o.vmode ? o.root.style.top : o.root.style.bottom);
            var x = parseInt(o.hmode ? o.root.style.left : o.root.style.right);
            var nx, ny;

            if (o.minX != null) ex = o.hmode ? Math.max(ex, o.minMouseX) : Math.min(ex, o.maxMouseX);
            if (o.maxX != null) ex = o.hmode ? Math.min(ex, o.maxMouseX) : Math.max(ex, o.minMouseX);
            if (o.minY != null) ey = o.vmode ? Math.max(ey, o.minMouseY) : Math.min(ey, o.maxMouseY);
            if (o.maxY != null) ey = o.vmode ? Math.min(ey, o.maxMouseY) : Math.max(ey, o.minMouseY);

            nx = x + ((ex - o.lastMouseX) * (o.hmode ? 1 : -1));
            ny = y + ((ey - o.lastMouseY) * (o.vmode ? 1 : -1));

            if (o.xMapper) nx = o.xMapper(y)
            else if (o.yMapper) ny = o.yMapper(x)

            Drag.obj.root.style[o.hmode ? "left" : "right"] = nx + "px";
            Drag.obj.root.style[o.vmode ? "top" : "bottom"] = ny + "px";
            Drag.obj.lastMouseX = ex;
            Drag.obj.lastMouseY = ey;

            Drag.obj.root.onDrag(nx, ny);

            return false;
        },
        end: function () {
            document.onmousemove = null;
            document.onmouseup = null;

            Drag.obj.root.onDragEnd(parseInt(Drag.obj.root.style[Drag.obj.hmode ? "left" : "right"]),
                parseInt(Drag.obj.root.style[Drag.obj.vmode ? "top" : "bottom"]));
            Drag.obj = null;
        },
        fixE: function (e) {
            if (typeof e == 'undefined') e = window.event;
            if (typeof e.layerX == 'undefined') e.layerX = e.offsetX;
            if (typeof e.layerY == 'undefined') e.layerY = e.offsetY;

            return e;
        }
    };

/* START: Java Applet functions */
var _enableApplet = true;
var _isAppletLoaded = false;
var _appletCheckInterval = null;
var _validateAppletJobData = {};
var _validateAppletWaitingCount = 0;
function validateAppletReady() {
    _isAppletLoaded = true;
}
function WaitForValidateAppletReady() {
    if (_validateAppletJobData.length = 0) { return; }

    if (_isAppletLoaded) {
        window.clearInterval(_appletCheckInterval);
        var a = document.applets["ValidateApplet"];
        if (a != null) {
            a.setUserAgent(navigator.userAgent);
        }
        appletValidate(_validateAppletJobData['szProtocol'], _validateAppletJobData['nPort'], _validateAppletJobData['szHTTPRequests']);
        _validateAppletJobData = {};
    } else if (_validateAppletWaitingCount > 30) {
        window.clearInterval(_appletCheckInterval);
    }
    _validateAppletWaitingCount++;
}

function loadJs(url) {
    var script = document.createElement('script');
    script.setAttribute('src', url);
    script.setAttribute('type', 'text/javascript');
    script.setAttribute('id', "depolyJava");

    var loaded = false;
    var loadFunction = function () {
        if (loaded) return;
        loaded = true;
    };
    script.onload = loadFunction;
    script.onreadystatechange = loadFunction;
    document.getElementsByTagName("head")[0].appendChild(script);
}

function initValidateApplet() {
    if (!document.getElementById('depolyJava')) {
        loadJs("images/deployJava.js");
    }
    if (!_isAppletLoaded) {
        var _apphtml = '<applet name="ValidateApplet" id="ValidateApplet" MAYSCRIPT="MAYSCRIPT" archive="images/Validate.jar" code="ValidateApplet.class" width="0" height="0"><param name="jnlp_href" value="images/launch.jnlp"/><param name="port" value="80"><param name="method" value="POST"><param name="contenttype" value="application/x-www-form-urlencoded"><param name="showbutton" value="0"></applet>';
        var _appspot = document.getElementById('ValidateAppletWrapper');
        _appspot.style.diplay = 'none';
        _appspot.innerHTML = _apphtml;
    }
}

function toggleValidateAppletEnable() {
    if (_enableApplet) {
        _enableApplet = false;
    }
    else {
        _enableApplet = true;
    }
}

function appletStart(e, szConnection, refAltValidate, bAbsolutelyNeedsApplet, szProtocol, nPort, szHTTPRequests) {

    // Just prefer to have ability to not set a bunch of null's
    if (typeof szConnection == "undefined") { szConnection = 'noapplet'; }
    if (typeof refAltValidate == "undefined") { refAltValidate = ''; }
    if (typeof bAbsolutelyNeedsApplet == "undefined") { bAbsolutelyNeedsApplet = false; }

    if (typeof szProtocol == "undefined") { szProtocol = 'http'; }
    if (typeof nPort == "undefined") { nPort = '80'; }
    if (typeof szHTTPRequests == "undefined") { szHTTPRequests = ''; }
    if (szHTTPRequests != '' && szProtocol == '') {
        szProtocol = 'http';
    }
    if (szHTTPRequests != '' && nPort == '') {
        nPort = '80';
    }

    var useApplet = _enableApplet;
    if (szConnection == 'noapplet') {
        useApplet = false;
    } else {
        if (e.which) {
            var rightclick = (e.which == 3);
        } else if (e.button) {
            var rightclick = (e.button == 2);
        }
        if (rightclick) {
            useApplet = false;
        }
    }

    cursor_wait();
    if (/Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor)) {
        notifyChromePlugin();
    }
    else if (useApplet) {
        _validateAppletWaitingCount = 0;
        _validateAppletJobData = {};
        _validateAppletJobData['refAltValidate'] = refAltValidate;

        _validateAppletJobData['szProtocol'] = szProtocol;
        _validateAppletJobData['nPort'] = nPort;
        _validateAppletJobData['szHTTPRequests'] = szHTTPRequests;

        _appletCheckInterval = window.setInterval(WaitForValidateAppletReady, 1000);
        initValidateApplet();
    }
    else if (bAbsolutelyNeedsApplet) {
        alert("We're sorry, the only way to validate this vulnerability is with the applet written for that purpose. Please try again and give the applet permission to run or view the report in a browser that can run the applet.");
    }
    else {
        browserValidate(refAltValidate);
    }
    cursor_clear();
}

function browserValidate(refAltValidate) {
    cursor_clear();
    if (document.getElementById(refAltValidate) == "undefined") {
        window.open(refAltValidate, 'ValidateBrowserWindow');
    } else if (document.getElementById(refAltValidate) == null) {
        window.open(refAltValidate, 'ValidateBrowserWindow');
    } else {
        submitForm(refAltValidate);
    }
}

function appletValidate(szProtocol, nPort, szHTTPRequests) {
    initValidateApplet();
    var a = document.getElementById('ValidateApplet');
    if (a != null) {
        a.MakeRequest(szProtocol, nPort, szHTTPRequests);
    }
    cursor_clear();
}
/* END: Java Applet functions */

/* START: Chrome Plugin functions */

function notifyChromePlugin() {
    var chromePluginImg = new Image();
    chromePluginImg.src = "chrome-extension://mnmlipalillmakdiildpclhocfgcddnp/assets/images/icon.png";
    chromePluginImg.onload = function () {};

    chromePluginImg.onerror = function () {
        if(confirm("For full functionality, please use the chrome plugin available. Do you want to install/enable this?"))
            window.open("https://chrome.google.com/webstore/detail/appspider-chrome-plugin/mnmlipalillmakdiildpclhocfgcddnp/");
    };
}

/* END: Chrome Plugin functions */