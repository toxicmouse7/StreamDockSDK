const $local = true, $back = false,
    $dom = {
        main: $('.sdpi-wrapper'),
        scriptPath: $('#path')
    },
    $propEvent = {
        didReceiveSettings() {
            if ($settings.scriptPath) {
                $dom.scriptPath.value = $settings.scriptPath;
            }
        }
    };

const onScriptPathChange = $.debounce(() => {
	console.log($dom.scriptPath);
	const payload = {
		"settings": {
			"scriptPath": $dom.scriptPath.value
		}
	};
    $websocket.sendToPlugin(payload);
}, 1000);

$dom.scriptPath.on("input", function () {
    onScriptPathChange();
});