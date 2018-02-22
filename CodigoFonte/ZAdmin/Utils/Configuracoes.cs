using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.Utils
{
    public static class Configuracoes
    {
        public static string retornaUrlAtual
        {
            get
            {
                var urlAtual = HttpContext.Current.Request.Url.AbsolutePath;
                return urlAtual;
            }
        }

        //public static string NomeUsuarioLogado
        //{
        //    get
        //    {

        //        if (HttpContext.Current.Session["usuario"] == null)
        //            return "";
        //        else
        //            return ((Usuario)HttpContext.Current.Session["usuario"]).nome;

        //    }
        //}

        //public static int idUsuarioLogado
        //{
        //    get
        //    {

        //        if (HttpContext.Current.Session["usuario"] == null)
        //            return 0;
        //        else
        //            return ((Usuario)HttpContext.Current.Session["usuario"]).idUsuario;

        //    }
        //}

        //public static string emailUsuarioLogado
        //{
        //    get
        //    {

        //        if (HttpContext.Current.Session["usuario"] == null)
        //            return null;
        //        else
        //            return ((Usuario)HttpContext.Current.Session["usuario"]).email;

        //    }
        //}


        public static string DiretorioImagens
        {
            get
            {
                var diretorioImagens = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioimagens"];
                    if (customSetting != null)
                        diretorioImagens = customSetting.Value;

                }

                return diretorioImagens;
            }
        }


        public static string ServidorElastic
        {
            get
            {
                var servidorElastic = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["serverElastic"];
                    if (customSetting != null)
                        servidorElastic = customSetting.Value;

                }

                return servidorElastic;
            }
        }

        public static string IndexElastic
        {
            get
            {
                var indexElastic = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["indexElastic"];
                    if (customSetting != null)
                        indexElastic = customSetting.Value;

                }

                return indexElastic;
            }
        }

        public static string DiretorioExecutaveis
        {
            get
            {
                var diretorioExecutaveis = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioExecutaveis"];
                    if (customSetting != null)
                        diretorioExecutaveis = customSetting.Value;

                }

                return diretorioExecutaveis;
            }
        }

        public static string DiretorioSalvarImagens
        {
            get
            {
                var diretorioSalvarImagens = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioSalvarImagens"];
                    if (customSetting != null)
                        diretorioSalvarImagens = customSetting.Value;

                }

                return diretorioSalvarImagens;
            }
        }


        public static string DiretorioSalvarArquivos
        {
            get
            {
                var diretorio = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioSalvarArquivos"];
                    if (customSetting != null)
                        diretorio = customSetting.Value;

                }

                return diretorio;
            }
        }


        public static string DiretorioArquivos
        {
            get
            {
                var diretorio = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioarquivos"];
                    if (customSetting != null)
                        diretorio = customSetting.Value;

                }

                return diretorio;
            }
        }
    }
}