/**
 * PropertyInspector 2.5.0 新特性 =>
 * 
 *      1 => 工具与主文件相分离 - 按需引入
 *      2 => $settings - 全局持久化数据代理 ※
 *      3 => 无需关注上下文 - 随时随地与插件通信
 *      4 => 注意事项: 为了避免命名冲突，请勿使用 $ 相关的名称以及JQuery库
 * 
 * ===== MiraBox ========================================== 2025.4.9 =====>
 */

let $websocket, $uuid, $action, $context, $settings, $lang;

// 设置全局持久化数据
WebSocket.prototype.setGlobalSettings = function(payload) {
    this.send(JSON.stringify({
        event: "setGlobalSettings",
        context: $uuid, payload
    }));
}

// 获取全局持久化数据
WebSocket.prototype.getGlobalSettings = function() {
    this.send(JSON.stringify({
        event: "getGlobalSettings",
        context: $uuid,
    }));
}

// 与插件通信
WebSocket.prototype.sendToPlugin = function (payload) {
    this.send(JSON.stringify({
        event: "sendToPlugin",
        action: $action,
        context: $uuid,
        payload
    }));
}

// 设置状态
WebSocket.prototype.setState = function (state) {
    this.send(JSON.stringify({
        event: "setState",
        context: $context,
        payload: { state }
    }));
}

// 设置背景
WebSocket.prototype.setImage = function (url) {
    let image = new Image();
    image.src = url;
    image.onload = () => {
        let canvas = document.createElement("canvas");
        canvas.width = image.naturalWidth;
        canvas.height = image.naturalHeight;
        let ctx = canvas.getContext("2d");
        ctx.drawImage(image, 0, 0);
        this.send(JSON.stringify({
            event: "setImage",
            context: $context,
            payload: {
                target: 0,
                image: canvas.toDataURL("image/png")
            }
        }));
    };
}

// 打开网页
WebSocket.prototype.openUrl = function (url) {
    this.send(JSON.stringify({
        event: "openUrl",
        payload: { url }
    }));
}

// 保存持久化数据
WebSocket.prototype.saveData = $.debounce(function (payload) {
    this.send(JSON.stringify({
        event: "setSettings",
        context: $uuid,
        payload
    }))
}, 0)

// StreamDock 软件入口函数
const connectSocket = connectElgatoStreamDeckSocket;
async function connectElgatoStreamDeckSocket(port, uuid, event, app, info) {
    info = JSON.parse(info);
    $uuid = uuid; $action = info.action; $context = info.context;
    $websocket = new WebSocket('ws://127.0.0.1:' + port);
    $websocket.onopen = () => $websocket.send(JSON.stringify({ event, uuid }));

    // 持久数据代理
    $websocket.onmessage = e => {
        let data = JSON.parse(e.data);
        if (data.event === 'didReceiveSettings') {
            $settings = new Proxy(data.payload.settings, {
                get(target, property) {
                    return target[property];
                },
                set(target, property, value) {
                    target[property] = value;
                    $websocket.saveData(data.payload.settings);
                }
            });
            if (!$back) $dom.main.style.display = 'block';
        }
        $propEvent[data.event]?.(data.payload);
    };

    // 自动翻译页面
    if (!$local) return;
    $lang = await new Promise(resolve => {
        const req = new XMLHttpRequest();
        req.open('GET', `../../${JSON.parse(app).application.language}.json`);
        req.send();
        req.onreadystatechange = () => {
            if (req.readyState === 4) {
                // console.log(req.responseText);
                resolve(JSON.parse(req.responseText).Localization)
            }
        };
    })

    // 遍历文本节点并翻译所有文本节点
    const walker = document.createTreeWalker($dom.main, NodeFilter.SHOW_TEXT, (e) => {
        return e.data.trim() && NodeFilter.FILTER_ACCEPT
    });
    while (walker.nextNode()) {
        console.log(walker.currentNode.data);
        walker.currentNode.data = $lang[walker.currentNode.data]
    }
    // placeholder 特殊处理
    const translate = item => {
        if (item.placeholder?.trim()) {
            console.log(item.placeholder);
            item.placeholder = $lang[item.placeholder]
        }
    }
    $('input', true).forEach(translate)
    $('textarea', true).forEach(translate)
}

// StreamDock 文件路径回调
let $FileID = ''; Array.from($('input[type="file"]', true)).forEach(item => {
    item.addEventListener('click', () => $FileID = item.id);
});
const onFilePickerReturn = (url) => $emit.send(`File-${$FileID}`, JSON.parse(url));