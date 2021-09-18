using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Livraria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "livraria";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }

        private void LimparCampos()
        {
            tbID.Clear();
            tbNome.Clear();
            tbGenero.Clear();
            tbDescricao.Clear();
            tbAno.Clear();
        }
        private void btLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            atualizarDataGrid();
        }

        private void atualizarDataGrid()
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());

            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM livros WHERE ativoLivro = 1";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgLivraria.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgLivraria.Rows[0].Clone();//FAZ UM CAST E CLONA A LINHA DA TABELA
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//NOME
                    row.Cells[2].Value = reader.GetString(2);//GENERO
                    row.Cells[3].Value = reader.GetString(3);//DESCRICAO
                    row.Cells[4].Value = reader.GetString(4);//ANO
                    dgLivraria.Rows.Add(row);//ADICIONO A LINHA NA TABELA
                }

                realizaConexacoBD.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível executar a ação.");
                Console.WriteLine(ex.Message);
            }
        }

        private void btInserir_Click(object sender, EventArgs e)
        {

            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();

                comandoMySql.CommandText = "INSERT INTO livros (nomelivro,generoLivro,descricaoLivro,anoLivro) " +
                    "VALUES('" + tbNome.Text + "', '" + tbGenero.Text + "','" + tbDescricao.Text + "', " + Convert.ToInt16(tbAno.Text) + ")";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Livro inserido com sucesso!");

                atualizarDataGrid();
                LimparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btAlterar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
                comandoMySql.CommandText = "UPDATE livros SET nomeLivro = '" + tbNome.Text + "', " +
                    "generoLivro = '" + tbGenero.Text + "', " +
                    "descricaoLivro = '" + tbDescricao.Text + "', " +
                    "anoLivro = " + Convert.ToInt16(tbAno.Text) +
                    " WHERE idLivro = " + tbID.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Livro atualizado com sucesso"); //Exibo mensagem de aviso
                atualizarDataGrid();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel abrir a conexão!");
                Console.WriteLine(ex.Message);
            }
        }

        private void btDeletar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
              
                comandoMySql.CommandText = "UPDATE livros SET ativoLivro = 0 WHERE idLivro = " + tbID.Text + "";

                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Livro deletado com sucesso"); //Exibo mensagem de aviso
                atualizarDataGrid();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel abrir a conexão!");
                Console.WriteLine(ex.Message);
            }
        }

        private void dgLivraria_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgLivraria.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgLivraria.CurrentRow.Selected = true;
                //preenche os textbox com as células da linha selecionada
                tbNome.Text = dgLivraria.Rows[e.RowIndex].Cells["colNome"].FormattedValue.ToString();
                tbGenero.Text = dgLivraria.Rows[e.RowIndex].Cells["colGenero"].FormattedValue.ToString();
                tbDescricao.Text = dgLivraria.Rows[e.RowIndex].Cells["colDescricao"].FormattedValue.ToString();
                tbAno.Text = dgLivraria.Rows[e.RowIndex].Cells["colAno"].FormattedValue.ToString();
                tbID.Text = dgLivraria.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
            }
        }

        private void dgLivraria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgLivraria.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgLivraria.CurrentRow.Selected = true;
                //preenche os textbox com as células da linha selecionada
                tbNome.Text = dgLivraria.Rows[e.RowIndex].Cells["colNome"].FormattedValue.ToString();
                tbGenero.Text = dgLivraria.Rows[e.RowIndex].Cells["colGenero"].FormattedValue.ToString();
                tbDescricao.Text = dgLivraria.Rows[e.RowIndex].Cells["colDescricao"].FormattedValue.ToString();
                tbAno.Text = dgLivraria.Rows[e.RowIndex].Cells["colAno"].FormattedValue.ToString();
                tbID.Text = dgLivraria.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
            }
        }
    }
}
