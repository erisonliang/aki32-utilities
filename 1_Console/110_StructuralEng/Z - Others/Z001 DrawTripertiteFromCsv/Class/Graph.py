import matplotlib.pyplot as plt
import numpy as np

class Graph:
    
    # ↓ for init -------------------------------------------------

    def __init__(self, item:dict):
        self.Dpi = item['dpi']
        self.Width = item['graph_width']
        self.Height = item['graph_height']
        self.MinPeriod = item['min_period']
        self.MaxPeriod = item['max_period']
        self.MinVel = item['min_vel']
        self.MaxVel = item['max_vel']
        self.XLabel = item['x_label']
        self.YLabel = item['y_label']
        self.AccLabel = item['acc_label']
        self.DispLabel = item['disp_label']
        self.EigenPeriod = item['eigen_period']

        self.fig = plt.figure(figsize=(self.Width, self.Height))
        self.ax = self.fig.add_subplot(1,1,1)
        plt.subplots_adjust(left=0.15)

        self.DrawGrids()


    def DrawGrids(self):
        x1 = self.MinPeriod
        x2 = self.MaxPeriod

        # acc
        acc_list = np.concatenate([np.linspace(int(10**i), int(10**(i+1)), 10) for i in range(4)])
        acc_lines = {}
        for acc in acc_list:
            # Sv = Sa / (2π/T)
            y1 = acc / (2.0 * np.pi / x1)
            y2 = acc / (2.0 * np.pi / x2)
            acc_lines[acc] = [x1, x2, y1, y2]
        self.DrawGrid('acc', self.AccLabel, acc_lines, [0.3, 0.65], 45, 'bottom')

        # disp
        disp_list = np.concatenate([np.linspace(0.01*int(10**i), 0.01*int(10**(i+1)), 10) for i in range(5)])
        disp_lines = {}
        for disp in disp_list:
            # Sv = Sd * (2π/T)
            y1 = disp * (2.0 * np.pi / x1)
            y2 = disp * (2.0 * np.pi / x2)
            disp_lines[disp] = [x1, x2, y1, y2]
        self.DrawGrid('disp', self.DispLabel, disp_lines, [0.7, 0.75], -45, 'top')


    def DrawGrid(self, grid_type, label, lines, text_pos, angle, vertalalign):
        for k, v in lines.items():
            x1, x2, y1, y2 = tuple(v)
            self.ax.plot([x1, x2], [y1, y2], color='darkgray', linewidth='0.5')
            text = '{:.0f}'.format(k)
            if (grid_type == 'acc'):
                if (np.log10(k)).is_integer():
                    self.ax.text(x1, y1, text, rotation=angle, verticalalignment=vertalalign)
            elif (grid_type == 'disp'):
                if k >= 1.0 and (np.log10(k)).is_integer():
                    self.ax.text(x2, y2, text, rotation=angle, verticalalignment=vertalalign)
        self.fig.text(text_pos[0], text_pos[1], label, rotation=angle)


    # ↑ for init -------------------------------------------------



    def DrawVelSpectrumLine(self, X, Y, label, color, lineStyle):
        self.ax.plot(X, Y, label=label, color=color, linestyle = lineStyle)

    def DrawPeriodLine(self):
        for period in self.EigenPeriod:
            self.ax.plot([period, period], [self.MinVel, self.MaxVel], color='black', linestyle='dashed')


    def Format(self):
        """Required to be called after all lines were drawn"""
        self.ax.set_xlim(self.MinPeriod, self.MaxPeriod)
        self.ax.set_ylim(self.MinVel, self.MaxVel)
        self.ax.set_xscale('log')
        self.ax.set_yscale('log')
        self.ax.grid(which="both")
        self.ax.legend(loc=4)
        self.ax.get_xaxis().set_major_formatter(plt.FormatStrFormatter('%.2f'))
        self.ax.get_yaxis().set_major_formatter(plt.FormatStrFormatter('%.2f'))
        self.ax.set_xlabel(self.XLabel)
        self.ax.set_ylabel(self.YLabel)

        















