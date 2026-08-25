function calculateMtpdCategory(Mtpd) {
    // null veya undefined kontrolü
    if (Mtpd == null) {
        return 0;
    }

    // Number'a çevirme
    Mtpd = Number(Mtpd);

    if (Mtpd >= 1 && Mtpd < 60) {
        return 4;
    } else if (Mtpd >= 60 && Mtpd < 480) {
        return 3;
    } else if (Mtpd >= 480 && Mtpd < 1440) {
        return 2;
    } else if (Mtpd >= 1440 && Mtpd <= 43200) {
        return 1;
    } else {
        return 0;
    }
}

function kriptolojiGereklilikKontrol(bilgiSinifiId) {
    const isKriptolojiGereksiz =
        bilgiSinifiId === 6 ||
        bilgiSinifiId === 4;
    return !isKriptolojiGereksiz;
}

$(document).ready(function () {

    if ($("#kriptolojiCheckbox").is(":checked")) {
        $("#kriptolojicard").show();
    } else {
        $("#kriptolojicard").hide();
    }

    $("#kriptolojiCheckbox").prop("disabled", true);

    var yedekleme = $("#YedeklemeTipiId").val();
    if (parseInt(yedekleme) === 1) {
        $("#yedeklemecard").hide();
    } else {
        $("#yedeklemecard").show();
    }

    $("#erisilebilirlikSelect").prop("disabled", true);

    $('#Rto').change(function () {
        var MtpdVal = $('#Mtpd').val();
        var rto = $('#Rto').val();

        if (!isNaN(rto) && !isNaN(MtpdVal) && parseInt(MtpdVal) !== 0 && parseInt(rto) > parseInt(MtpdVal)) {
            Swal.fire({
                title: 'Hata',
                text: 'MTPD değeri RTO değerinden küçük olamaz',
                icon: 'error',
                confirmButtonText: 'Tamam',
                customClass: {
                    confirmButton: 'btn btn-primary'
                }
            });
        }
    });

    $('#Mtpd').change(function () {
        var MtpdVal = $('#Mtpd').val();
        var rto = $('#Rto').val();

        if (isNaN(MtpdVal)) {
            return;
        }

        if (!isNaN(rto) && parseInt(rto) > parseInt(MtpdVal)) {
            Swal.fire({
                title: 'Hata',
                text: 'MTPD değeri RTO değerinden küçük olamaz',
                icon: 'error',
                confirmButtonText: 'Tamam',
                customClass: {
                    confirmButton: 'btn btn-primary'
                }
            });
        }

        $("#erisilebilirlikSelect").prop("disabled", false);
        var erisilebilirlikId = calculateMtpdCategory(parseInt(MtpdVal));
        $('#erisilebilirlikSelect').val(erisilebilirlikId).trigger('change');
        $('#ErisilebilirlikId').val(erisilebilirlikId);
        $("#erisilebilirlikSelect").prop("disabled", true);

    });

    $('#BilgiSinifiId').change(function () {
        var bilgiSinifiId = $('#BilgiSinifiId').val();
        if (isNaN(bilgiSinifiId)) {
            return;
        }
        $("#kriptolojiCheckbox").prop("disabled", false);
        var kriptolojiVal = kriptolojiGereklilikKontrol(parseInt(bilgiSinifiId));
        $('#kriptolojiCheckbox').prop('checked', kriptolojiVal);
        $('#Kriptoloji').val(kriptolojiVal);
        $("#kriptolojiCheckbox").prop("disabled", true);

        if (kriptolojiVal) {
            $("#kriptolojicard").show();
        } else {
            $("#kriptolojicard").hide();
        }
    });

    $('#YedeklemeTipiId').change(function () {
        var id = $("#YedeklemeTipiId").val();

        if (Number(id) == 1 || id == "" || isNaN(id)) {
            $("#yedeklemecard").hide();
        } else {
            $("#yedeklemecard").show();
        }
    });
});


function deleteRecord(url) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu kaydı silmek istediğinizden emin misiniz?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST',
                success: function (data) {
                    if (data && data.success === false) {
                        Swal.fire({
                            title: 'Erişim Engellendi!',
                            text: data.message || 'İşlem başarısız.',
                            icon: 'error',
                            confirmButtonText: 'Tamam'
                        });
                        return;
                    }
                    Swal.fire({
                        title: 'Silindi!',
                        text: 'Kayıt başarıyla silindi.',
                        icon: 'success',
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() => {
                        window.location.reload();
                    });
                },
                error: function () {
                    Swal.fire({
                        title: 'Hata!',
                        text: 'Silme işlemi sırasında bir hata oluştu.',
                        icon: 'error',
                        confirmButtonText: 'Tamam'
                    });
                }
            });
        }
    });
}

function deleteDatabaseRecord(url) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu kaydı kalıcı olarak silmek istediğinizden emin misiniz?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST',
                success: function (data) {
                    if (data && data.success === false) {
                        Swal.fire({
                            title: 'Erişim Engellendi!',
                            text: data.message || 'İşlem başarısız.',
                            icon: 'error',
                            confirmButtonText: 'Tamam'
                        });
                        return;
                    }
                    Swal.fire({
                        title: 'Silindi!',
                        text: 'Kayıt veritabanından başarıyla silindi.',
                        icon: 'success',
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() => {
                        window.location.reload();
                    });
                },
                error: function () {
                    Swal.fire({
                        title: 'Hata!',
                        text: 'Silme işlemi sırasında bir hata oluştu.',
                        icon: 'error',
                        confirmButtonText: 'Tamam'
                    });
                }
            });
        }
    });
}

$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});