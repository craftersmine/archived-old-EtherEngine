namespace craftersmine.EtherEngine.Core
{
    partial class AcceleratedCanvas
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.canvasRoot = new System.Windows.Forms.Integration.ElementHost();
            this.acceleratedCanvasBase = new craftersmine.EtherEngine.Core.AcceleratedXamlCanvasBase();
            this.SuspendLayout();
            // 
            // canvasRoot
            // 
            this.canvasRoot.BackColor = System.Drawing.Color.Transparent;
            this.canvasRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasRoot.Location = new System.Drawing.Point(0, 0);
            this.canvasRoot.Name = "canvasRoot";
            this.canvasRoot.Size = new System.Drawing.Size(406, 395);
            this.canvasRoot.TabIndex = 0;
            this.canvasRoot.Child = this.acceleratedCanvasBase;
            // 
            // AcceleratedCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.canvasRoot);
            this.Name = "AcceleratedCanvas";
            this.Size = new System.Drawing.Size(406, 395);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost canvasRoot;
        private AcceleratedXamlCanvasBase acceleratedCanvasBase;
    }
}
