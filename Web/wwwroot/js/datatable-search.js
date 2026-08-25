$.fn.dataTable.ext.search.push(
    function(settings, data, dataIndex) {
        var searchText = $('.dataTables_filter input').val().toLowerCase();

        // Boş arama değeri varsa tüm verileri göster
        if (!searchText) return true;

        // Tüm sütunlarda arama yap
        for (var i = 0; i < data.length; i++) {
            if (data[i].toLowerCase().includes(searchText)) {
                return true;
            }
        }
        return false;
    }
);