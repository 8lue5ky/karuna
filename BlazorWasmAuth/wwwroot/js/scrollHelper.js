// registriert den Scroll-Listener auf window
window.registerScrollHandler = function (dotnetRef) {
    window.addEventListener("scroll", () => {
        dotnetRef.invokeMethodAsync("OnWindowScroll", {
            scrollTop: window.pageYOffset,
            windowHeight: window.innerHeight,
            scrollHeight: document.documentElement.scrollHeight
        });
    });
};

window.registerPullToRefresh = function (dotnetRef) {
    let startY = 0;
    let pulling = false;
    const threshold = 70;

    window.addEventListener("touchstart", e => {
        if (window.scrollY === 0) {
            startY = e.touches[0].clientY;
            pulling = true;
        }
    });

    window.addEventListener("touchmove", e => {
        if (!pulling) {
            return;
        }

        let dist = e.touches[0].clientY - startY;

        if (dist > 0) {

            dotnetRef.invokeMethodAsync("OnPullProgress", dist);

            if (dist > threshold) {
                pulling = false;
                dotnetRef.invokeMethodAsync("OnPullTriggered");
            }
        }
    });

    window.addEventListener("touchend", e => {
        if (pulling) {
            dotnetRef.invokeMethodAsync("OnPullProgress", 0);
        }
        pulling = false;
    });
};
