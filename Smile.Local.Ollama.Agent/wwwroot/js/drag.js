window.dragInterop = {
    enableDrag: function (element, dotNetRef, shapeId) {
        const canvas = element.closest(".canvas");
        let offsetX = 0, offsetY = 0;

        element.onmousedown = function (e) {
            const rect = canvas.getBoundingClientRect();
            offsetX = e.clientX - element.getBoundingClientRect().left;
            offsetY = e.clientY - element.getBoundingClientRect().top;

            document.onmousemove = function (e) {
                const x = e.clientX - rect.left - offsetX;
                const y = e.clientY - rect.top - offsetY;
                element.style.transform = `translate(${x}px, ${y}px)`;
                dotNetRef.invokeMethodAsync("OnDrag", shapeId, x, y);
            };

            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
            };
        };
    }
};