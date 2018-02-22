function validarCNPJ(valcnpj) {

    if (valcnpj == '') {
        return false;
    }
    else if (valcnpj.length != 14) {
        if (valcnpj.length > 1) {
            return false;
        }
    }

    if (valcnpj == "00000000000000" ||
        valcnpj == "11111111111111" ||
        valcnpj == "22222222222222" ||
        valcnpj == "33333333333333" ||
        valcnpj == "44444444444444" ||
        valcnpj == "55555555555555" ||
        valcnpj == "66666666666666" ||
        valcnpj == "77777777777777" ||
        valcnpj == "88888888888888" ||
        valcnpj == "99999999999999") {
        return false;
    }

    if (valcnpj.length == 14) {

        var tamanho = valcnpj.length - 2
        var numeros = valcnpj.substring(0, tamanho);
        var digitos = valcnpj.substring(tamanho);
        var soma = 0;
        var pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) {
                pos = 9;
            }
        }

        var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(0)) {
            return false;
        }

        tamanho = tamanho + 1;
        numeros = valcnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) {
                pos = 9;
            }
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(1)) {
            return false;
        }

        return true;
    }

}