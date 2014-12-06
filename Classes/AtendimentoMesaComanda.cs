﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysFood.Classes
{
    class AtendimentoMesaComanda
    {
        Classes.Banco clBanco = new Classes.Banco();

        //Variáveis Atendimento_MesaComanda
        public static int idatt;

        private string datacadastro;
        public string Datacadastro
        {
            get { return datacadastro; }
            set { datacadastro = value; }
        }

        private string mesacomanda;
        public string Mesacomanda
        {
            get { return mesacomanda; }
            set { mesacomanda = value; }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        //Variáveis Atendimento_MesaComandaItens
        private int barras;
        public int Barras
        {
            get { return barras; }
            set { barras = value; }
        }

        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        private decimal quantidade;
        public decimal Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        private decimal precounitario;
        public decimal Precounitario
        {
            get { return precounitario; }
            set { precounitario = value; }
        }

        private decimal totalgeral;
        public decimal Totalgeral
        {
            get { return totalgeral; }
            set { totalgeral = value; }
        }

        //Variáveis Contas a Receber
        private int finalizadora;
        public int Finalizadora
        {
            get { return finalizadora; }
            set { finalizadora = value; }
        }

        private int parcelas;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        private string descfinalizadora;
        public string Descfinalizadora
        {
            get { return descfinalizadora; }
            set { descfinalizadora = value; }
        }

        private int cliente;
        public int Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        int aux;

        public void Salvar()
        {
            clBanco.Conectar();
            MySqlCommand busca = new MySqlCommand("SELECT * FROM atendimento_mesacomanda WHERE mesacomanda = '"+ Mesacomanda +"' AND aberta = 0;", clBanco.Conn);
            MySqlDataReader dr = busca.ExecuteReader();
            if (dr.Read())
            {
                aux = Convert.ToInt32(dr["id"]);
                string sql2 = "INSERT INTO `atendimento_mesacomanda_item` (`at_mesacomanda_id`, `codigodebarras`, `descricao`, `quantidade`, `precounitario`, `total`) " +
                "VALUES ('" + aux + "', '" + Barras + "', '" + Descricao + "', '" + Quantidade + "', REPLACE ('" + Precounitario + "', ',', '.'), REPLACE ('" + Total + "', ',', '.'));";

                clBanco.Executar(sql2);
            }
            //return dr;

            else
            {
                string sql = "INSERT INTO `atendimento_mesacomanda` (`datacadastro`, `mesacomanda`, `total`, `aberta`,`status`) " +
                    "VALUES ('" + Datacadastro + "', '" + Mesacomanda + "', '" + Total + "', '" + 0 + "','" + Status + "');";
                //passa aberta = 0 para indicar aberta e dar select sempre nas abertas ja que o número da comanda poderá
                //repetir e causar problemas na inserção dos itens

                aux = clBanco.runQueryFetchId(sql); // Executa a função junto ao comando acima.
                Forms.FrmFinalizadora.vendafinalizadora = aux;

                string sql2 = "INSERT INTO `atendimento_mesacomanda_item` (`at_mesacomanda_id`, `codigodebarras`, `descricao`, `quantidade`, `precounitario`, `total`) " +
                    "VALUES ('" + aux + "', '" + Barras + "', '" + Descricao + "', '" + Quantidade + "', REPLACE ('" + Precounitario + "', ',', '.'), REPLACE ('" + Total + "', ',', '.'));";

                clBanco.Executar(sql2);
            }
        }

        public DataTable Exibir()
        {
            DataTable dt = new DataTable();
            try
            {
                clBanco.Conectar();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM vmesacomanda WHERE mesacomanda = '" + Mesacomanda + "' AND aberta = 0;", clBanco.Conn);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = sql;
                da.Fill(dt);
            }
            catch (Exception erro)
            {
                MessageBox.Show("" + erro);
            }
            finally
            {
                clBanco.FecharConexao();
            }
            return dt;
        }

        public void Deletar()
        {
            try
            {
                string sql = "DELETE FROM atendimento_mesacomanda_item WHERE id = '" +  + "'";
                clBanco.Executar(sql);
                //MessageBox.Show("Dados excluídos com sucesso!", "Sucesso.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Erro ao executar comando de exclusão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clBanco.FecharConexao();
            }
        }
    }
}
