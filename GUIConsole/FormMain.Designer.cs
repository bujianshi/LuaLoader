namespace LuaHotLoader
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.fctb = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tv = new System.Windows.Forms.TreeView();
            this.contextMenuTVNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.luaStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerHor = new System.Windows.Forms.SplitContainer();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ctrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.realodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cfgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.netToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerVer = new System.Windows.Forms.SplitContainer();
            this.richTextLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fctb)).BeginInit();
            this.contextMenuTVNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHor)).BeginInit();
            this.splitContainerHor.Panel1.SuspendLayout();
            this.splitContainerHor.Panel2.SuspendLayout();
            this.splitContainerHor.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVer)).BeginInit();
            this.splitContainerVer.Panel1.SuspendLayout();
            this.splitContainerVer.Panel2.SuspendLayout();
            this.splitContainerVer.SuspendLayout();
            this.SuspendLayout();
            // 
            // fctb
            // 
            this.fctb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fctb.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctb.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fctb.BackBrush = null;
            this.fctb.CharHeight = 14;
            this.fctb.CharWidth = 8;
            this.fctb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctb.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctb.ImeMode = System.Windows.Forms.ImeMode.On;
            this.fctb.IsReplaceMode = false;
            this.fctb.Location = new System.Drawing.Point(3, 0);
            this.fctb.Name = "fctb";
            this.fctb.Paddings = new System.Windows.Forms.Padding(0);
            this.fctb.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctb.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb.ServiceColors")));
            this.fctb.Size = new System.Drawing.Size(548, 400);
            this.fctb.TabIndex = 0;
            this.fctb.Zoom = 100;
            // 
            // tv
            // 
            this.tv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv.ContextMenuStrip = this.contextMenuTVNode;
            this.tv.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tv.LabelEdit = true;
            this.tv.Location = new System.Drawing.Point(0, 0);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(276, 400);
            this.tv.TabIndex = 1;
            this.tv.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tv_AfterLabelEdit);
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            // 
            // contextMenuTVNode
            // 
            this.contextMenuTVNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPathToolStripMenuItem,
            this.newFileToolStripMenuItem,
            this.luaStateToolStripMenuItem});
            this.contextMenuTVNode.Name = "contextMenuTVNode";
            this.contextMenuTVNode.Size = new System.Drawing.Size(128, 70);
            // 
            // newPathToolStripMenuItem
            // 
            this.newPathToolStripMenuItem.Name = "newPathToolStripMenuItem";
            this.newPathToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.newPathToolStripMenuItem.Text = "NewPath";
            this.newPathToolStripMenuItem.Click += new System.EventHandler(this.newPathToolStripMenuItem_Click);
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.newFileToolStripMenuItem.Text = "NewFile";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
            // 
            // luaStateToolStripMenuItem
            // 
            this.luaStateToolStripMenuItem.Name = "luaStateToolStripMenuItem";
            this.luaStateToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.luaStateToolStripMenuItem.Text = "LuaState";
            this.luaStateToolStripMenuItem.MouseEnter += new System.EventHandler(this.luaStateToolStripMenuItem_MouseEnter);
            // 
            // splitContainerHor
            // 
            this.splitContainerHor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHor.Location = new System.Drawing.Point(3, 3);
            this.splitContainerHor.Name = "splitContainerHor";
            // 
            // splitContainerHor.Panel1
            // 
            this.splitContainerHor.Panel1.Controls.Add(this.tv);
            // 
            // splitContainerHor.Panel2
            // 
            this.splitContainerHor.Panel2.AutoScroll = true;
            this.splitContainerHor.Panel2.Controls.Add(this.fctb);
            this.splitContainerHor.Size = new System.Drawing.Size(830, 400);
            this.splitContainerHor.SplitterDistance = 275;
            this.splitContainerHor.TabIndex = 3;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctrlToolStripMenuItem,
            this.cfgToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(836, 25);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "menuStrip";
            // 
            // ctrlToolStripMenuItem
            // 
            this.ctrlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.closeConnectionToolStripMenuItem,
            this.realodToolStripMenuItem,
            this.logToolStripMenuItem});
            this.ctrlToolStripMenuItem.Name = "ctrlToolStripMenuItem";
            this.ctrlToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.ctrlToolStripMenuItem.Text = "控制";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.connectToolStripMenuItem.Text = "连接";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // closeConnectionToolStripMenuItem
            // 
            this.closeConnectionToolStripMenuItem.Enabled = false;
            this.closeConnectionToolStripMenuItem.Name = "closeConnectionToolStripMenuItem";
            this.closeConnectionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeConnectionToolStripMenuItem.Text = "断开";
            this.closeConnectionToolStripMenuItem.Click += new System.EventHandler(this.closeConnectionToolStripMenuItem_Click);
            // 
            // realodToolStripMenuItem
            // 
            this.realodToolStripMenuItem.Name = "realodToolStripMenuItem";
            this.realodToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.realodToolStripMenuItem.Text = "重载";
            this.realodToolStripMenuItem.Click += new System.EventHandler(this.realodToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logToolStripMenuItem.Text = "日志";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // cfgToolStripMenuItem
            // 
            this.cfgToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.netToolStripMenuItem});
            this.cfgToolStripMenuItem.Name = "cfgToolStripMenuItem";
            this.cfgToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.cfgToolStripMenuItem.Text = "配置";
            // 
            // netToolStripMenuItem
            // 
            this.netToolStripMenuItem.Name = "netToolStripMenuItem";
            this.netToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.netToolStripMenuItem.Text = "网络";
            this.netToolStripMenuItem.Click += new System.EventHandler(this.netToolStripMenuItem_Click);
            // 
            // splitContainerVer
            // 
            this.splitContainerVer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerVer.Location = new System.Drawing.Point(0, 28);
            this.splitContainerVer.Name = "splitContainerVer";
            this.splitContainerVer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerVer.Panel1
            // 
            this.splitContainerVer.Panel1.Controls.Add(this.splitContainerHor);
            // 
            // splitContainerVer.Panel2
            // 
            this.splitContainerVer.Panel2.Controls.Add(this.richTextLog);
            this.splitContainerVer.Size = new System.Drawing.Size(836, 510);
            this.splitContainerVer.SplitterDistance = 406;
            this.splitContainerVer.TabIndex = 6;
            // 
            // richTextLog
            // 
            this.richTextLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextLog.AutoWordSelection = true;
            this.richTextLog.Location = new System.Drawing.Point(3, -3);
            this.richTextLog.Name = "richTextLog";
            this.richTextLog.Size = new System.Drawing.Size(830, 96);
            this.richTextLog.TabIndex = 0;
            this.richTextLog.Text = "";
            this.richTextLog.TextChanged += new System.EventHandler(this.richTextLog_TextChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 533);
            this.Controls.Add(this.splitContainerVer);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.Text = "MAIN";
            ((System.ComponentModel.ISupportInitialize)(this.fctb)).EndInit();
            this.contextMenuTVNode.ResumeLayout(false);
            this.splitContainerHor.Panel1.ResumeLayout(false);
            this.splitContainerHor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHor)).EndInit();
            this.splitContainerHor.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainerVer.Panel1.ResumeLayout(false);
            this.splitContainerVer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVer)).EndInit();
            this.splitContainerVer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox fctb;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.SplitContainer splitContainerHor;
        private System.Windows.Forms.ContextMenuStrip contextMenuTVNode;
        private System.Windows.Forms.ToolStripMenuItem newPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem luaStateToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem ctrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem realodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cfgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem netToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerVer;
        private System.Windows.Forms.RichTextBox richTextLog;
    }
}

