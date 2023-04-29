
# ★★★★★★★★★★★★★★★ imports

# 基本
import numpy as np
import pandas as pd
import os
import pathlib

# 可視化ライブラリ
import matplotlib.pyplot as plt
import matplotlib.patches as patches


class SNAPBeamVisualizer():

    # ★★★★★★★★★★★★★★★ props

    # ★★★★★ 手動入力
    x_axes = 9
    y_axes = 4
    x_building_size = 51.2
    y_building_size = 35.2
    diagram_zoom = 3

    grid_size = 3

    # ★★★★★ 自動算出
    x_diagram_size = grid_size * x_axes - grid_size + 1
    y_diagram_size = grid_size * y_axes - grid_size + 1
    x_zoom = x_diagram_size * diagram_zoom
    y_zoom = y_diagram_size * diagram_zoom

    result_excel_dir_path = ""
    result_excel_file_name_without_extension = ""


    # ★★★★★★★★★★★★★★★ init

    def __init__(self, building_excel_path, result_excel_path, grid_size=3) -> None:

        # ★★★★★ 何もないならスルー
        if building_excel_path=="":
            return 

        # ★★★★★ 出力情報
        self.grid_size = grid_size

        # ★★★★★ 構造物情報

        # 読み込み
        self.df_node = pd.read_excel(building_excel_path, sheet_name="節点")
        self.df_beam = pd.read_excel(building_excel_path, sheet_name="梁")
        self.df_axis = pd.read_excel(building_excel_path, sheet_name="軸")

        # 前処理（節点）
        self.df_node = pd.merge(self.df_node, self.df_axis[['#axis', 'x']], on='x', how='left').rename(columns={'#axis':'x_#axis'})
        self.df_node = pd.merge(self.df_node, self.df_axis[['#axis', 'y']], on='y', how='left').rename(columns={'#axis':'y_#axis'})
        self.df_node = pd.merge(self.df_node, self.df_axis[['#axis', 'z']], on='z', how='left').rename(columns={'#axis':'z_#axis'})

        # 前処理（梁）
        size = max(self.df_beam.count())
        self.df_beam['#beam'] = range(1, size+1)
        self.df_beam = self.df_beam \
            .drop(["i","j","一本指定","一本_i","一本_j"],axis=1) \
            .dropna()
        df_node_temp = self.df_node[['#node', 'x_#axis', 'y_#axis', 'z_#axis']]
        self.df_beam = pd.merge(self.df_beam, df_node_temp, left_on='i_#node', right_on='#node', how='left').drop(columns=['#node']).rename(columns={'x_#axis':'i_x_#axis', 'y_#axis':'i_y_#axis', 'z_#axis':'i_z_#axis'})
        self.df_beam = pd.merge(self.df_beam, df_node_temp, left_on='j_#node', right_on='#node', how='left').drop(columns=['#node']).rename(columns={'x_#axis':'j_x_#axis', 'y_#axis':'j_y_#axis', 'z_#axis':'j_z_#axis'})

        # ★★★★★ 解析結果情報

        # 結果出力用フォルダの作成
        result_excel_fi = pathlib.Path(result_excel_path)
        self.result_excel_dir_path = result_excel_fi.parent
        self.result_excel_file_name_without_extension = result_excel_fi.stem
        
        # 読み込み
        self.df_mu = pd.read_excel(result_excel_path, sheet_name="塑性変形頻度")
        self.df_damage = pd.read_excel(result_excel_path, sheet_name="損傷")

        pass

    # ★★★★★★★★★★★★★★★ 可視化

    # ★★★★★ フレーム

    def VisualizeFrame(self, skip_show=False):

        # def
        self.InitSharedFig()

        # output
        plt.savefig(self.GetResultImagePath("Frame", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return


    # ★★★★★ 梁

    def VisualizeBeamName_CUI(self, target_story, skip_show=False):

        # def
        beam_num_grid = self.InitSharedCUIGrid(target_story=target_story)
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beam（一本部材はi端部材から順に入力することを利用）
        def ProcessOne(beam_num, edge_name):
            try:
                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_num, edge_name)
                beam_num_grid[x_diagram][y_diagram] = beam_num
            except:
                None

        beam_num_list = df_target_beams[f"#beam"].dropna().tolist()
        for beam_num in beam_num_list:
            ProcessOne(beam_num, "j") # 常に最後の番号を採用
        beam_num_list.reverse()
        for beam_num in beam_num_list:
            ProcessOne(beam_num, "i") # 常に最初の番号を採用

        # output
        beam_num_grid = [list(x) for x in zip(*beam_num_grid)]
        pd.DataFrame(beam_num_grid).to_csv(self.GetResultCsvPath("BeamName", ""))
        if skip_show==False:
            for line in beam_num_grid:
                for d in line:
                    if type(d) is np.int64: d = str(d)
                    if type(d) is int: d = str(d)
                    print(d.center(6), end="")
                print()
                print()
            print()
        return

    def VisualizeBeamName(self, target_story, skip_show=False):

        # def
        self.InitSharedFig()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beams（一本部材はi端部材から順に入力することを利用）
        def ProcessOne(beam_num, edge_name):
            try:
                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_num=beam_num, edge_name=edge_name)

                ax = self.AddAxesToSharedFig(x_diagram,y_diagram)
                colors = ["snow"] # ghostwhite snow
                x = np.array([1])
                _, texts = \
                    ax.pie(x, colors=colors, radius=1, labeldistance=0,
                    wedgeprops={'linewidth': 1, 'edgecolor':"black"}, labels=[str(beam_num)+edge_name])
                for t in texts:
                    t.set_verticalalignment('center')
                    t.set_horizontalalignment('center')
                    t.set_size(40)
                    
            except:
                None

        beam_num_list = df_target_beams[f"#beam"].dropna().tolist()
        for beam_num in beam_num_list:
            ProcessOne(beam_num, "j") # 常に最後の番号を採用
        beam_num_list.reverse()
        for beam_num in beam_num_list:
            ProcessOne(beam_num, "i") # 常に最初の番号を採用

        # output
        plt.savefig(self.GetResultImagePath("BeamName", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return


    # ★★★★★ 損傷

    def GetSpecificBeamDamage_CUI(self, beam_edge_name):
        damage = self.df_damage[beam_edge_name].iloc[0]
        return damage
        
    def VisualizeBeamDamage_CUI(self, target_story, skip_show=False):

        # def
        damage_grid = self.InitSharedCUIGrid()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beam all
        for d_index in self.df_damage.columns:
            try:
                # 諸元抜き出し
                damage = self.df_damage[d_index].iloc[0]
                (beam_num, edge_name) = self.GetSeparatedBeamEdgeName(d_index)

                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_num, edge_name)
                if damage > 0:
                    damage_grid[x_diagram][y_diagram] = float(damage)

            except:
                None

        # output
        damage_grid = [list(x) for x in zip(*damage_grid)]
        pd.DataFrame(damage_grid).to_csv(self.GetResultCsvPath("BeamDamage", ""))
        if skip_show==False:
            for line in damage_grid:
                for d in line:
                    if type(d) is float:
                        d = "{:.3f}".format(d)
                    if type(d) is np.int64:
                        d = str(d)
                    print(d.center(6), end="")
                print()
                print()
            print()
        return

    def VisualizeBeamDamage(self, target_story, skip_show=False):

        # def
        self.InitSharedFig()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beams
        for d_index in self.df_damage.columns:
            try:
                # 諸元抜き出し
                damage = self.df_damage[d_index].iloc[0]

                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_edge_name=d_index)
                if damage > 0:
                    ax = self.AddAxesToSharedFig(x_diagram,y_diagram)

                    colors = [self.GetGradColor(damage,1)] # ghostwhite snow
                    x = np.array([1])
                    _, texts = \
                        ax.pie(x, colors=colors, radius=1, labeldistance=0,
                        wedgeprops={'linewidth': 1, 'edgecolor':"black"}, labels=["{:.2f}".format(damage*100)]) # "{:.2f}".format(mus[-1]) # str(int(100*damage))
                    for t in texts:
                        t.set_verticalalignment('center')
                        t.set_horizontalalignment('center')
                        t.set_size(30) # 50
                    
            except:
                None

        # output
        plt.savefig(self.GetResultImagePath("BeamDamage", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return


    # ★★★★★ 塑性率分布

    def DrawSpecificBeamMuAmpHist(self, ax, mus):

        # ヒストグラム参考 https://www.yutaka-note.com/entry/matplotlib_hist
        n, bins, _ = ax.hist(mus,             
            histtype="stepfilled",
            orientation="horizontal",
            range=(0.1,10),
            # bins=np.logspace(-1, 1, num=20), # 0.1 - 10
            bins=99,
            color="blue" # "green"
            )
        ax.set_xscale('log')
        ax.set_yscale('log')
        ax.set_xlim(0.1, 1000)
        ax.set_ylim(0.1, 10)

        return (n, bins)

    def VisualizeSpecificBeamMuAmpHist(self, beam_edge_name, skip_show=False):
        
        ax = plt.figure().add_subplot()
        mus = self.df_mu[beam_edge_name].dropna().tolist()
        
        n, bins = self.DrawSpecificBeamMuAmpHist(ax, mus)
        pd.DataFrame([n, bins]).to_csv(self.GetResultCsvPath("BeamMuAmpHist", beam_edge_name, more_dirs="SpecificBeamMuAmpHist\\"))


        # output
        plt.savefig(self.GetResultImagePath("BeamMuAmpHist", beam_edge_name, more_dirs="SpecificBeamMuAmpHist\\"))
        if skip_show==False:
            plt.show()
        plt.close()
        return

    def VisualizeSpecificBeamMuAmpHist_Loop(self, target_story, skip_show=False):
        df_target_beams = self.GetSpecificStoryBeams(target_story)
        for beam in df_target_beams["#beam"]:
            for suf in ["_i", "_j"]:
                try:
                    self.VisualizeSpecificBeamMuAmpHist(beam_edge_name=(f"{beam}{suf}"), skip_show=skip_show)
                except:
                    plt.close()
                    None
        return

    def VisualizeBeamMuAmpHist_Test(self, skip_show=False):

        # def
        self.InitSharedFig()
        gs = self.grid_size

        # beam X
        for bx in range(0,self.x_axes-1):
            for by in range(0,self.y_axes):
                self.AddAxesToSharedFig(bx*gs   +1 , by*gs).plot([0,1,2],[0,1,8], color="red")
                self.AddAxesToSharedFig(bx*gs+gs-1 , by*gs).plot([0,1,2],[0,1,8], color="red")

        # beam Y
        for bx in range(0,self.x_axes):
            for by in range(0,self.y_axes-1):
                self.AddAxesToSharedFig(bx*gs, by*gs   +1 ).plot([0,1,8],[0,1,2], color="blue")
                self.AddAxesToSharedFig(bx*gs, by*gs+gs-1 ).plot([0,1,8],[0,1,2], color="blue")

        # output
        plt.savefig(self.GetResultImagePath("BeamMuAmpHist_Test", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return

    def VisualizeBeamMuAmpHist(self, target_story, skip_show=False):

        # def
        self.InitSharedFig()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beams
        for mu_index in self.df_mu.columns:
            try:
                # 諸元抜き出し
                mus = self.df_mu[mu_index].dropna().tolist()

                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_edge_name=mu_index)
                if len(mus) > 0:
                    ax = self.AddAxesToSharedFig(x_diagram,y_diagram)
                    self.DrawSpecificBeamMuAmpHist(ax, mus)

            except:
                None

        # output
        plt.savefig(self.GetResultImagePath("BeamMuAmpHist", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return


    # ★★★★★ 最大塑性率

    def VisualizeBeamMuAmpPeak_CUI(self, target_story, skip_show=False):

        # def
        damage_grid = self.InitSharedCUIGrid()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beam all
        for mu_index in self.df_mu.columns:
            try:
                # 諸元抜き出し
                mus = self.df_mu[mu_index].dropna().tolist()
                (beam_num, edge_name) = self.GetSeparatedBeamEdgeName(mu_index)

                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_num, edge_name)
                if len(mus) > 0:
                    damage_grid[x_diagram][y_diagram] = float(mus[-1]) # "{:.2f}".format(mus[-1])

            except:
                None

        # output
        damage_grid = [list(x) for x in zip(*damage_grid)]
        pd.DataFrame(damage_grid).to_csv(self.GetResultCsvPath("BeamMuAmpPeak", ""))
        if skip_show==False:
            for line in damage_grid:
                for d in line:
                    if type(d) is float:
                        d = "{:.3f}".format(d)
                    if type(d) is np.int64:
                        d = str(d)
                    print(d.center(6), end="")
                print()
                print()
            print()
        return

    def VisualizeBeamMuAmpPeak(self, target_story, skip_show=False):

        # def
        self.InitSharedFig()
        df_target_beams = self.GetSpecificStoryBeams(target_story)

        # beams
        for mu_index in self.df_mu.columns:
            try:
                # 諸元抜き出し
                mus = self.df_mu[mu_index].dropna().tolist()
                (beam_num, edge_name) = self.GetSeparatedBeamEdgeName(mu_index)

                # 表に追記
                x_diagram, y_diagram = self.GetDiagram_xy(df_target_beams, beam_num, edge_name)
                if len(mus) > 0:
                    ax = self.AddAxesToSharedFig(x_diagram,y_diagram)
                    colors = [self.GetGradColor(mus[-1],10)] # ghostwhite snow
                    x = np.array([1])
                    _, texts = \
                        ax.pie(x, colors=colors, radius=1, labeldistance=0,
                        wedgeprops={'linewidth': 1, 'edgecolor':"black"}, labels=["{:.2f}".format(mus[-1])])
                    for t in texts:
                        t.set_verticalalignment('center')
                        t.set_horizontalalignment('center')
                        t.set_size(40)

            except:
                None

        # show
        plt.savefig(self.GetResultImagePath("BeamMuAmpPeak", ""))
        if skip_show==False:
            plt.show()
        plt.close()
        return


    # ★★★★★★★★★★★★★★★ グラフ用関数たち

    def InitSharedCUIGrid(self, with_column=True, target_story=None):

        # def
        self.shared_grid = [[" "]*self.y_diagram_size for a in range(self.x_diagram_size)]
        gs = self.grid_size

        # columns
        if with_column:
            for bx in range(0,self.x_axes):
                for by in range(0,self.y_axes):

                    if target_story==None:
                        self.shared_grid[bx*gs][by*gs] = "C"
                    else:
                        node_num = self.df_node\
                            [self.df_node["x_#axis"]==bx+1]\
                            [self.df_node["y_#axis"]==by+1]\
                            [self.df_node["z_#axis"]==target_story]\
                            ["#node"].iloc[0]
                        self.shared_grid[bx*gs][by*gs] = node_num

        return self.shared_grid

    def InitSharedFig(self, with_columns=True, with_beams=True):

        # def
        self.shared_fig = plt.figure(figsize=(self.x_zoom, self.y_zoom))
        gs = self.grid_size
        
        # beams
        if with_beams:

            bw = 0.25 # beam_width
            bl = 0.96 # beam_length

            # beam X
            for by in range(0,self.y_axes):
                ax = self.shared_fig.add_subplot(self.y_diagram_size, 1, by*gs+1)
                ax.add_patch(patches.Rectangle(xy=((1-bl)/2, (1-bw)/2), width=bl, height=bw, color="gray", fill=True))
                ax.set_xlim(0,1)
                ax.set_ylim(0,1)
                ax.get_xaxis().set_visible(False)
                ax.get_yaxis().set_visible(False)
                ax.set_frame_on(False)

            # beam Y
            for bx in range(0,self.x_axes):
                ax = self.shared_fig.add_subplot(1, self.x_diagram_size, bx*gs+1)
                ax.add_patch(patches.Rectangle(xy=((1-bw)/2, (1-bl)/2), width=bw, height=bl, color="gray", fill=True))
                ax.set_xlim(0,1)
                ax.set_ylim(0,1)
                ax.get_xaxis().set_visible(False)
                ax.get_yaxis().set_visible(False)
                ax.set_frame_on(False)

        # columns
        if with_columns:
            cs = 0.80 # column_size

            for bx in range(0,self.x_axes):
                for by in range(0,self.y_axes):
                    ax = self.AddAxesToSharedFig(bx*gs,by*gs)
                    ax.add_patch(patches.Rectangle(xy=((1-cs)/2, (1-cs)/2), width=cs, height=cs, color="gray", fill=True)) # ec='black'
                    ax.set_xlim(0,1)
                    ax.set_ylim(0,1)
                    ax.get_xaxis().set_visible(False)
                    ax.get_yaxis().set_visible(False)
                    ax.set_frame_on(False)

        return self.shared_fig

    def GetResultImagePath(self, image_type_string, file_suffix, more_dirs=""):
        new_dir_path = f"{self.result_excel_dir_path}\\ResultImage\\"
        new_dir_path += more_dirs        
        os.makedirs(new_dir_path, exist_ok=True)
        return f"{new_dir_path}\\{image_type_string}_{self.result_excel_file_name_without_extension}_{file_suffix}.png"

    def GetResultCsvPath(self, csv_type_string, file_suffix, more_dirs=""):
        new_dir_path = f"{self.result_excel_dir_path}\\ResultCsv\\"
        new_dir_path += more_dirs        
        os.makedirs(new_dir_path, exist_ok=True)
        return f"{new_dir_path}\\{csv_type_string}_{self.result_excel_file_name_without_extension}_{file_suffix}.csv"

    def AddAxesToSharedFig(self, x, y):
        ax = self.shared_fig.add_subplot(
            self.y_diagram_size, 
            self.x_diagram_size, 
            self.x_diagram_size * y + x + 1)

        return ax

    def GetGradColor(self, value, max):
        per = float(value)/max
        if(per>1) :per=1
        if(per<0) :per=0
        return (1,1-per,1-per)


    # ★★★★★★★★★★★★★★★ ユーティリティたち

    def Conv_ij(self, ij):
        return "j" if ij == "i" else "i"

    def GetDiagram_xy(self, df_target_beams, beam_num="", edge_name="", beam_edge_name=""):
        if beam_edge_name != "":
            beam_num, edge_name = self.GetSeparatedBeamEdgeName(beam_edge_name)
        df_target_beam = df_target_beams[df_target_beams["#beam"]==beam_num]
        this_xa = df_target_beam[f"{edge_name}_x_#axis"].iloc[0]
        this_ya = df_target_beam[f"{edge_name}_y_#axis"].iloc[0]
        over_xa = df_target_beam[f"{self.Conv_ij(edge_name)}_x_#axis"].iloc[0]
        over_ya = df_target_beam[f"{self.Conv_ij(edge_name)}_y_#axis"].iloc[0]
        x =  this_xa * (self.grid_size-1) + over_xa - self.grid_size
        y =  this_ya * (self.grid_size-1) + over_ya - self.grid_size
        return (x,y)

    def GetSeparatedBeamEdgeName(self, beam_edge_name):
        sep = beam_edge_name.split(sep="_")
        beam_num = int(sep[0])
        edge_name = sep[1]
        return (beam_num, edge_name)

    def GetSpecificStoryBeams(self, target_story):
        return self.df_beam[self.df_beam['i_z_#axis']==target_story]


    # ★★★★★★★★★★★★★★★
