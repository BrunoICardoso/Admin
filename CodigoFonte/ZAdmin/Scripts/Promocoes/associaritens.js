function ContainerAssociador(configs) {

    var urlPromocoes = configs.urlPromocoes;
    var modulo = configs.modulo;

    this.Inicializa = function () {
        var theLoc = $('.tabs-associacao').offset().top - 10;
        var largura = $('.tabs-associacao').width();
        $('.tabs-associacao').css('width', largura);

        if (modulo == "noticias") {
            $("#tituloRecurso").text("Notícias por promoção");
        }
        else {
            $("#tituloRecurso").text("Posts por promoção");
        }


        $(window).scroll(function () {
            if (theLoc >= $(window).scrollTop()) {
                if ($('.tabs-associacao').hasClass('container-movel')) {
                    $('.tabs-associacao').removeClass('container-movel');
                }
            } else {
                if (!$('.tabs-associacao').hasClass('container-movel')) {
                    $('.tabs-associacao').addClass('container-movel');
                }
            }
        });

        CarregaComboEmpresaPromocoes();

        $("#cmbEmpresaPromocoes").on('change', function (event, params) {
            var idempresa = $("#cmbEmpresaPromocoes").val();
            CarregaListaPromocoesEmpresa(idempresa);
        });

        ReajustaTamanho();

        $(window).scroll(function () {
            ReajustaTamanho();
        });

    }


    function ReajustaTamanho() {
        var loc = $(window).height() - ($('.lista-promos').offset().top - $(window).scrollTop()) - 40;
        $('.lista-promos').css('max-height', loc);
    }


    function CarregaComboEmpresaPromocoes() {

        var uri = "/api/PromocaoAPI/GetEmpresas";

        $.getJSON(uri)
              .done(function (data) {

                  $.each(data, function (key, item) {
                      $("#cmbEmpresaPromocoes").append("<option value=" + item.idItem + ">" + item.nome + "</option>");
                      $("#cmbEmpresaPromocoes").trigger('chosen:updated');
                  });
              });
    }


    function CarregaListaPromocoesEmpresa(idempresa) {
        
        var uri = urlPromocoes + idempresa;
        $("#tabela-promo").empty();
        $.getJSON(uri)
              .done(function (data) {

                  $.each(data, function (key, item) {

                      //$("#tabela-promo").append("<tr class='linha-promo' draggable='true' data-idpromocao='" + item.idItem + "' ondrop='drop(event, this)' ondragover='allowDrop(event)'> <td id='nomeLinhaPromo'>" + item.nome + "</td> <td class='num-rel'>" + item.total + "</td></tr>");
                      //$("#tabela-promo").append("<tr class='linha-promo' draggable='true' data-idpromocao='" + item.idItem + "' ondrop='drop(event, this)'> <td>" + item.nome + "</td> <td class='num-rel'>" + item.total + "</td></tr>");

                      var _onClick = "";
                      if (modulo == 'redessociais') {
                          _onClick = 'ClickPesquisarRedesSociasPorPromocao(' + item.idItem + ',' + item.total + ')';
                      }

                      $("#tabela-promo").append("<tr onClick='" + _onClick + "' class='linha-promo' draggable='true' data-idpromocao='" + item.idItem + "' ondrop='drop(event, this)' ondragover='allowDrop(event)'> <td id='nomeLinhaPromo'>" + item.nome + "</td> <td class='num-rel'>" + item.total + "</td></tr>");
                      //$("#tabela-promo").append("<tr class='linha-promo' draggable='true' data-idpromocao='" + item.idItem + "' ondrop='drop(event, this)'> <td>" + item.nome + "</td> <td class='num-rel'>" + item.total + "</td></tr>");
                      
                      EventHouver(event);

                  });

                  $("#tabela-promo .linha-promo").click(function () {
                      $("#tabela-promo .linha-promo").removeClass('selected');

                      $(this).addClass('selected');


                   
                      if (ClicouPromocao != undefined)
                      ClicouPromocao($(this).attr("data-idpromocao"));
                  })
              })
    }
    
    var ClicouPromocao;

    this.setClicouPromocao = function (event) {

        ClicouPromocao = event;

    }

}

function EventHouver(event) {


//-------------------------
//    function allowDrop(event) {

//        event.preventDefault();
//    }
// Esse evento faz a troca no mouse foi para _IndexTabelaNoticias e _ListaRedesSociais por causa do event no botão  no ondraover='allowDrop(event)'
//-----------------------------------

  
    function handleDragOver(event) {
    
        if (event.preventDefault) {
            event.preventDefault(); 
        }

        event.dataTransfer.dropEffect = 'move';  

        return false;
    }

    function handleDragEnter(event) {
    
        this.classList.add('over');
        //console.log('entro');
    
    }

    function handleDragLeave(event) {
    
        this.classList.remove('over');
        //console.log('saiu');

    }


    function handleDrop(event) {

        if (event.stopPropagation) {
            event.stopPropagation();
        }

        return false;
    }

    function handleDragEnd(event) {
        
        [].forEach.call(cols, function (col) {
            
            col.classList.remove('over');
        });
    }

    var cols = document.querySelectorAll('tr[class="linha-promo"]');

    [].forEach.call(cols, function (col) {
        //  debugger;
        //col.addEventListener('dragstart', handleDragStart, false);
        col.addEventListener('dragenter', handleDragEnter, false)
        col.addEventListener('dragover', handleDragOver, false);
        col.addEventListener('dragleave', handleDragLeave, false);
        col.addEventListener('drop', handleDrop, false);
        col.addEventListener('dragend', handleDragEnd, false);
    });
}

function AssociaPromoNoticias(idPromocao, idNoticia, idFontePesquisa, nomePromocao, elemento) {


    $.ajax({
        type: "GET",
        url: "/api/PromocaoNoticiaAPI/atualizaPromocaoNoticia?idPromocao=" + idPromocao + "&idFontePesquisa=" + idFontePesquisa + "&idNoticia=" + idNoticia,
        success: function (data) {
            if (data.Codigo == 1) {

                $("#avisoSemResultado" + idNoticia).remove();
                
                var qtdPromocoes = $(elemento).find(".num-rel").text();
                $(elemento).find(".num-rel").text(parseInt(qtdPromocoes) + 1);
            

                var tag = "<div class='boxTagPromos' id='tag_" + idPromocao + "_" + idNoticia + "'><div class='btn btn-block btn-social btnBoxPromocao'>" +
                           "<span class='fa fa-times btnBoxPromocaoClick' onclick='DesassociarPromocaoNoticia(" + idPromocao + "," + idNoticia + ")' ></span>" + nomePromocao + " </div></div>";

                $("#tagsPromocoes" + idNoticia).append(tag);
            }
        }
    });
}

function AssociaPromoRedesSociais(idPromocao, idPost, nomeRede, elemento) {

    var dados = {
        idpost: idPost,
        idpromocao: idPromocao,
        nomeRede: nomeRede
    }

    $.ajax({
        type: "POST",
        url: "/api/PromocaoRedeSocialAPI/AssociarPromocaoPost",
        contentType: 'application/json',
        data: JSON.stringify(dados),
        dataType: 'json',
        success: function (nomePromocao) {


          
            if (nomePromocao != "") {
                var totalAtual = $(elemento).find('td.num-rel').html();
                totalAtual = parseInt(totalAtual) + 1;
                $(elemento).find('td.num-rel').html(totalAtual);

                //if ($('#listaPromocao' + idPost).html().indexOf('Não há promoções') != -1) {
                //    $('#listaPromocao' + idPost).html('');
                //}

                if ($('#listaPromocao' + idPost).children('.btnBoxPromocao').length == 0) {
                    $('#listaPromocao' + idPost).html('');
                }


                $('#listaPromocao' + idPost).append('<div class="btn btn-block btn-social btnBoxPromocao tag_' + idPromocao + '_' + idPost + '" style="display: inline-block">' +
                                                        '<span class="fa fa-times btnBoxPromocaoClick" onclick="DesassociarPromocaoPost(' + idPromocao + ',' + idPost + ',\'' + nomeRede + '\' )"></span> ' + nomePromocao +
                                                   '</div>');
            }


        }
    });
}
