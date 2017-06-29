using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using LuaHotLoader.LogSys;
using LuaHotLoader.NetWork;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;

namespace LuaHotLoader
{
    public partial class FormMain : Form
    {
        private Net net;
        private string ip = "127.0.0.1";
        private int port = 5566;
        private Dictionary<uint, uint> dictStateRef = new Dictionary<uint, uint>();

        private delegate void DelCreateTreeNode(JObject jsonObj);

        public FormMain()
        {
            InitializeComponent();
            fctb_SetLuaStyle();

            Log.DelLogWarn = LogWarn;
            Log.DelLogError = LogEror;
            Log.DelLogDebug = LogDebug;
        }

        private void fctb_SetLuaStyle()
        {
            fctb.ClearStylesBuffer();
            fctb.Range.ClearStyle(StyleIndex.All);
            fctb.AddStyle(new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray))));
            fctb.OnSyntaxHighlight(new TextChangedEventArgs(fctb.Range));
            fctb.Language = Language.Lua;
            fctb.ImeMode = ImeMode.On;
        }

        private void LogEror(string err)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogEror), err);
            }
            else
            {
                richTextLog.AppendText("error: " + err + "\r\n");
            }
        }

        private void LogWarn(string warn)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogWarn), warn);
            }
            else
            {
                richTextLog.AppendText("warn: " + warn + "\r\n");
            }
        }

        private void LogDebug(string info)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogDebug), info);
            }
            else
            {
                richTextLog.AppendText("debug: " + info + "\r\n");
            }
        }

        private void OnPackage(JObject jsonObj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<JObject>(OnPackage), jsonObj);
            }
            else
            {
                Log.LogD("chunk " + jsonObj["chunk_name"]);
                CreateContentMenu(jsonObj);
                CreateTreeNode(jsonObj);
            }
        }

        private void OnError()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnError));
            }
            else
            {
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["connectToolStripMenuItem"].Enabled = true;
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["closeConnectionToolStripMenuItem"].Enabled = false;
                Log.LogD("connection error!");
            }
        }

        private void OnClosed()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnClosed));
            }
            else
            {
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["connectToolStripMenuItem"].Enabled = true;
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["closeConnectionToolStripMenuItem"].Enabled = false;
                Log.LogD("connection closed!");
            }
        }

        private void OnConnected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnConnected));
            }
            else
            {
                fctb.Text = "";
                tv.Nodes.Clear();
                dictStateRef.Clear();

                ((ToolStripMenuItem) contextMenuTVNode.Items["luaStateToolStripMenuItem"]).DropDownItems.Clear();
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["connectToolStripMenuItem"].Enabled = false;
                ((ToolStripMenuItem) menuStrip.Items["ctrlToolStripMenuItem"]).DropDownItems["closeConnectionToolStripMenuItem"].Enabled = true;

                Log.LogD("connected!");
            }
        }

        private void CreateContentMenu(JObject jsonObj)
        {
            var l = (uint) jsonObj["l"];

            if (!dictStateRef.ContainsKey(l))
            {
                dictStateRef.Add(l, 1);

                var menuState = contextMenuTVNode.Items["luaStateToolStripMenuItem"];
                var dropMenu = (ToolStripDropDownItem) menuState;
                var subItem = new ToolStripMenuItem(l.ToString("X8"));
                subItem.CheckOnClick = true;
                subItem.Click += delegate(object o, EventArgs args)
                {
                    if (tv.SelectedNode != null && tv.SelectedNode.Tag != null && tv.SelectedNode.Tag is JObject)
                    {
                        ((JObject) tv.SelectedNode.Tag)["l"] = l;
                    }
                };
                subItem.MouseEnter += delegate(object sender, EventArgs args)
                {
                    if (tv.SelectedNode != null && tv.SelectedNode.Tag != null && tv.SelectedNode.Tag is JObject)
                    {
                        subItem.ToolTipText = dictStateRef[l].ToString();
                    }
                };
                dropMenu.DropDownItems.Add(subItem);
            }
            else
            {
                dictStateRef[l]++;
            }
        }

        private void CreateTreeNode(JObject jsonObj)
        {
            try
            {
                TreeNode node = null;
                var nodes = tv.Nodes;
                var nodeNames = jsonObj["chunk_name"].ToString().Split('/');
                for (int i = 0; i < nodeNames.Length; i++)
                {
                    var nodeName = nodeNames[i];
                    if (string.IsNullOrEmpty(nodeName.Trim(new[] {' '})))
                    {
                        continue;
                    }

                    bool exists = false;
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        if (nodes[j].Text == nodeName)
                        {
                            node = nodes[j];
                            nodes = node.Nodes;
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        node = new TreeNode(nodeName);
                        nodes.Add(node);
                        nodes = node.Nodes;
                    }
                }

                if (node != null)
                {
                    node.Tag = jsonObj;
                    if (node.IsSelected)
                    {
                        fctb.Text = jsonObj["chunk_content"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogE("create node exp " + e);
            }
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                fctb.Enabled = true;
                var jsonObj = e.Node.Tag as JObject;
                if (jsonObj != null)
                {
                    fctb.Text = jsonObj["chunk_content"].ToString();
                }
            }
            else
            {
                fctb.Enabled = false;
            }
        }

        private void tv_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is JObject)
            {
                e.Node.Text = e.Label ?? e.Node.Text;
                (e.Node.Tag as JObject)["chunk_name"] = e.Node.FullPath.Replace(@"\", @"/");
                Log.LogD("modify chunk name to " + (e.Node.Tag as JObject)["chunk_name"]);
            }
        }

        private void newPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode newPathNode = null;

            if (tv.SelectedNode == null)
            {
                newPathNode = tv.Nodes.Add("NewPath");
            }
            else
            {
                if (tv.SelectedNode.Tag == null)
                {
                    tv.SelectedNode.Expand();
                    newPathNode = tv.SelectedNode.Nodes.Add("NewPath");
                }
            }

            if (newPathNode != null)
            {
                newPathNode.BeginEdit();
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode newChunkNode = null;

            if (tv.SelectedNode == null)
            {
                newChunkNode = tv.Nodes.Add("NewFile");
            }
            else
            {
                if (tv.SelectedNode.Tag == null)
                {
                    tv.SelectedNode.Expand();
                    newChunkNode = tv.SelectedNode.Nodes.Add("NewFile");
                }
            }

            if (newChunkNode != null)
            {
                var data = new JObject();
                data["l"] = 0;
                data["chunk_name"] = newChunkNode.FullPath;
                data["chunk_content"] = "";
                uint maxRef = 0;
                uint maxRefState = 0;
                foreach (var stateEntry in dictStateRef)
                {
                    if (stateEntry.Value > maxRef)
                    {
                        maxRefState = stateEntry.Key;
                        maxRef = stateEntry.Value;
                    }
                }
                data["l"] = maxRefState;
                newChunkNode.Tag = data;
                tv.SelectedNode = newChunkNode;
                newChunkNode.BeginEdit();
            }
        }

        private void luaStateToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null && tv.SelectedNode.Tag is JObject)
            {
                var dropMenu = sender as ToolStripDropDownItem;
                if (dropMenu != null)
                {
                    for (int i = 0; i < dropMenu.DropDownItems.Count; i++)
                    {
                        var curNodeL = (uint) ((JObject) tv.SelectedNode.Tag)["l"];
                        if (dropMenu.DropDownItems[i].Text.EndsWith(curNodeL.ToString("X8")))
                        {
                            ((ToolStripMenuItem) dropMenu.DropDownItems[i]).Checked = true;
                            break;
                        }
                    }
                }
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            net = new Net(ip, port);
            net.EventOnPackage += OnPackage;
            net.EventOnConnected += OnConnected;
            net.EventOnError += OnError;
            net.EventOnClosed += OnClosed;
            net.Connect();
        }

        private void closeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (net != null)
            {
                net.DisConnect();
            }
        }

        private void netToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inputStr = Interaction.InputBox("Connect URL:", "Net Config", "127.0.0.1:5566");
            if (!string.IsNullOrEmpty(inputStr))
            {
                var splits = inputStr.Split(':');
                try
                {
                    ip = splits[0];
                    port = int.Parse(splits[1]);
                    Log.LogD("reset net cfg: " + inputStr);
                }
                catch (Exception exp)
                {
                    Log.LogE("set net cfg exp " + exp);
                }
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory+"\\log.bat");
        }

        private void realodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null)
            {
                if (tv.SelectedNode.Tag != null && tv.SelectedNode.Tag is JObject)
                {
                    var jsonObj = tv.SelectedNode.Tag as JObject;
                    jsonObj["chunk_content"] = fctb.Text;
                    net.Send(jsonObj);
                    Log.LogD("try realod chunk "+jsonObj["chunk_name"]);
                }
            }
        }

        private void richTextLog_TextChanged(object sender, EventArgs e)
        {
            richTextLog.SelectionStart = richTextLog.Text.Length; //Set the current caret position at the end
            richTextLog.ScrollToCaret(); //Now scroll it automatically
        }
    }
}
