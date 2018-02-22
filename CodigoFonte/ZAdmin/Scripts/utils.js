

function formataDataJson(dateJson) {
    if (dateJson != null) {

        var dataehora = dateJson.split("T");
        var data = dataehora[0].split("-");
        var hora = dataehora[1].split(":");

        var dd = data[2];
        var mm = data[1];
        var yy = data[0];

        var dataFormatada = dd + '/' + mm + '/' + yy;

        return dataFormatada;
    } else {
        return "01/01/0001";
    }
}

function ConverteDataStringParaData(d) {

    var date = d.split("/");

    var dd = date[0];
    var mm = date[1];
    var yy = date[2];
    //var fromdt = new Date(Date.UTC(yy, mm - 1, dd, 00, 00, 00));
    var fromdt = new Date(yy, mm - 1, dd, 00, 00, 00);

    return fromdt;
}

// Recebe 17
// Retorna 2017
function tratarDataAnoDoisDigitos(d) {
    if (d != null) {
        var tmp = d.split("/");
        return tmp[0] + "/" + tmp[1] + "/" + "20" + tmp[2];
    }
    return 0;
}

function formataNumero(n) {
    return n.toString().replace(/[.]/g, ",").replace(/\d(?=(?:\d{3})+(?:\D|$))/g, "$&.");
}

function ConverteDataParaDataJson(d) {
    var dd = ("0" + d.getDate()).slice(-2);
    var mm = ("0" + (d.getMonth() + 1)).slice(-2);
    var yy = d.getFullYear();

    var hh = ("0" + d.getHours()).slice(-2);
    var min = ("0" + d.getMinutes()).slice(-2);
    var ss = ("0" + d.getSeconds()).slice(-2);

    var fromdt = yy + "-" + mm + "-" + dd + "T" + hh + ":" + min + ":" + ss + "";

    return fromdt;
}

// Recebe 2017-02-06
// retorna 06/02/2017
function formataDataBancoparaBR(d) {
    var tmp = d.split("-");
    return tmp[2] + "/" + tmp[1] + "/" + tmp[0];
}


//Formata as datas no formato YYYY-MM-DD e apresenta no formato dd/MM/YYYY
function formataDataInvertida(dataInvertida) {

    var data = dataInvertida.split("-");

    var dd = data[2];
    var mm = data[1];
    var yy = data[0];

    var dataFormatada = dd + '/' + mm + '/' + yy;

    return dataFormatada;
}