const tableWrapper = document.getElementById('tableWrapper');
const leftScrollBar = document.getElementById('leftScrollBar');
const rightScrollBar = document.getElementById('rightScrollBar');

if (tableWrapper && leftScrollBar && rightScrollBar) {
    // Kaydırma çubuklarını başlangıçta güncelleyelim
    function updateScrollbars() {
        // Kaydırılabilir alan var mı kontrol et
        const scrollableWidth = tableWrapper.scrollWidth - tableWrapper.clientWidth;

        if (scrollableWidth <= 0) {
            // Kaydırılabilir alan yoksa çubukları devre dışı bırak
            leftScrollBar.disabled = true;
            rightScrollBar.disabled = true;
            return;
        } else {
            leftScrollBar.disabled = false;
            rightScrollBar.disabled = false;
        }

        const scrollPercentage = Math.round((tableWrapper.scrollLeft / scrollableWidth) * 100);
        leftScrollBar.value = scrollPercentage;
        rightScrollBar.value = scrollPercentage;
    }

    // Sol kaydırma çubuğu değiştiğinde
    leftScrollBar.addEventListener('input', function () {
        const scrollableWidth = tableWrapper.scrollWidth - tableWrapper.clientWidth;
        const scrollPosition = Math.round((this.value / 100) * scrollableWidth);
        tableWrapper.scrollLeft = scrollPosition;
        rightScrollBar.value = this.value; // Sağ çubuğu da senkronize et
    });

    // Sağ kaydırma çubuğu değiştiğinde
    rightScrollBar.addEventListener('input', function () {
        const scrollableWidth = tableWrapper.scrollWidth - tableWrapper.clientWidth;
        const scrollPosition = Math.round((this.value / 100) * scrollableWidth);
        tableWrapper.scrollLeft = scrollPosition;
        leftScrollBar.value = this.value; // Sol çubuğu da senkronize et
    });

    // Tablo içeriği kaydırıldığında çubukları güncelle
    tableWrapper.addEventListener('scroll', function () {
        updateScrollbars();
    });

    // Sayfa yüklendiğinde çubukları güncelle
    window.addEventListener('load', function () {
        updateScrollbars();
    });

    // Pencere boyutu değiştiğinde çubukları güncelle
    window.addEventListener('resize', function () {
        updateScrollbars();
    });
}