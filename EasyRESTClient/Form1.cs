using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace EasyRESTClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbHttpMethod.DataSource = Enum.GetValues(typeof(httpVerb));
            DataGridViewRow KullaniciAdi = new DataGridViewRow();
            dgvHeaders.Rows.Add(3);
            dgvHeaders.Rows[0].Cells[0].Value = "KullaniciAdi";
            dgvHeaders.Rows[1].Cells[0].Value = "Sifre";
            dgvHeaders.Rows[2].Cells[0].Value = "APIKey";


        }


        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("explorer", "https://github.com/umutsurmeli/EasyRESTClient");
            }
            catch(Exception ex)
            {
                txtErros.AppendText(ex.Message + Environment.NewLine);
            }
            
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //try
            //{
                RestClient rClient = new RestClient();
                rClient.endPoint = txtRequestURI.Text;
                rClient.httpMethodSec(cmbHttpMethod.SelectedValue.ToString());
                //MessageBox.Show();

                string strResponse = string.Empty;

                strResponse = rClient.makeRequest(getHeadersList(),getDataList());

                txtResponse.Text = strResponse;
            
        /*}
            catch (Exception ex)
            {
                txtErros.AppendText(ex.Message + Environment.NewLine);
            }
        */
        }
        private List<KeyValuePair<string, string>> getHeadersList()
        {
            var requestHeaders = new List<KeyValuePair<string, string>>();
            string HeaderName;
            string HeaderValue;
            int headersLength = dgvHeaders.Rows.Count;
            
            for (int i=0;i< headersLength; i++)
            {
                var tst1 = dgvHeaders.Rows[i].Cells[0].Value;
                if(tst1==null)
                {
                    continue;
                }

                HeaderName = dgvHeaders.Rows[i].Cells[0].Value.ToString().Trim();
                var tst2 = dgvHeaders.Rows[i].Cells[1].Value;
                if (tst2 != null)
                {
                    HeaderValue = dgvHeaders.Rows[i].Cells[1].Value.ToString().Trim();
                }
                else
                {
                    HeaderValue = "";
                }
                if(HeaderName.Length>0)
                {
                    requestHeaders.Add(new KeyValuePair<string, string>(HeaderName, HeaderValue));
                }

            }

            return requestHeaders;
        }
        private List<KeyValuePair<string, string>> getDataList()
        {
            var requestData = new List<KeyValuePair<string, string>>();
            string DataName;
            string DataValue;
            int DataLength = dgvBody.Rows.Count;

            for (int i = 0; i < DataLength; i++)
            {
                var tst1 = dgvBody.Rows[i].Cells[0].Value;
                if (tst1 == null)
                {
                    continue;
                }

                DataName = dgvBody.Rows[i].Cells[0].Value.ToString().Trim();
                var tst2 = dgvBody.Rows[i].Cells[1].Value;
                if (tst2 != null)
                {
                    DataValue = dgvBody.Rows[i].Cells[1].Value.ToString().Trim();
                }
                else
                {
                    DataValue = "";
                }
                if (DataName.Length > 0)
                {
                    requestData.Add(new KeyValuePair<string, string>(DataName, DataValue));
                }

            }

            return requestData;
        }


    }
}
