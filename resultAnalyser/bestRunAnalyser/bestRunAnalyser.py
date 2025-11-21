import pandas as pd


def concat_files(file_paths, output_file=None):
    dfs = []

    for file_path in file_paths:
        df = pd.read_csv(file_path)
        dfs.append(df)

    merged = pd.concat(dfs, ignore_index=True)

    if output_file:
        merged.to_csv(output_file, index=False)

    return merged

def create_latex_chart(filename, caption, label, y_label, x_label, data, legend_entry, symbolic_x_coords=("20","40","80")):
    if data.__len__() != legend_entry.__len__():
        print("Długość danych i legendy muszą być takie same")
        return
    with open(filename, "w") as f:
        f.write("\\begin{figure}[H]\n")
        f.write("\\centering\n")
        f.write("\\begin{tikzpicture}\n")
        f.write("\\begin{axis}[\n")
        f.write(f"xlabel = {{{x_label}}},\n")
        f.write(f"ylabel = {{{y_label}}},\n")
        f.write("legend pos = north west,\n")
        f.write("grid = both,\n")
        f.write("width=1\\linewidth,\n")
        f.write("height=0.5\\linewidth,\n")
        f.write("ybar,\n")
        f.write("bar width=15pt,\n")
        f.write("symbolic x coords={")
        for coord in symbolic_x_coords:
            f.write(f"{coord},")
        f.write("},\n")
        f.write("xtick=data\n")
        f.write("]\n")
        for i in range(data.__len__()):
            f.write("\\addplot + [mark = *, thick] coordinates\n")
            f.write("    {\n")

            str = ""
            for key in data[i].keys():
                str += f"({key},{data[i][key]})"
            f.write(f"{str}")
            f.write("};\n")
            f.write("\\addlegendentry\n")
            f.write(f"{{{legend_entry[i]}}}\n")
        f.write("\\end{axis}\n")
        f.write("\\end{tikzpicture}\n")
        f.write(f"\\caption\n{{{caption}}}\n")
        f.write(f"\\label{{{label}}}\n")
        f.write("\\end{figure}\n")

def concat_files_mean(file_path, output_file=None, value = "GrandTotal"):
    # value to co ma byc srednia
    df = pd.read_csv(file_path)
    result = {
        "C1": [],
        "C2": [],
        "R1": [],
        "R2": [],
        "RC1": [],
        "RC2": [],
    }
    for idx, row in df.iterrows():
        filename = row["File"]
        val = float(row[value])
        if ("C1" in filename and "RC1" not in filename):
            result["C1"].append(val)
        if ("C2" in filename and "RC2" not in filename):
            result["C2"].append(val)
        if ("R1" in filename):
            result["R1"].append(val)
        if ("R2" in filename):
            result["R2"].append(val)
        if ("RC1" in filename):
            result["RC1"].append(val)
        if ("RC2" in filename):
            result["RC2"].append(val)
    for key in result:
        values = result[key]
        result[key] = sum(values) / len(values) if len(values) > 0 else None


    return result
def create_comparison_table(greedy_file, tabu_file, bee_file, output_file="comparison.csv"):
    df_greedy = pd.read_csv(greedy_file)
    df_tabu = pd.read_csv(tabu_file)
    df_bee = pd.read_csv(bee_file)

    df_greedy = df_greedy[["File", "GrandTotal"]].rename(columns={"GrandTotal": "GrandTotal_Greedy"})
    df_tabu = df_tabu[["File", "GrandTotal"]].rename(columns={"GrandTotal": "GrandTotal_Tabu"})
    df_bee = df_bee[["File", "GrandTotal"]].rename(columns={"GrandTotal": "GrandTotal_Bee"})

    merged = df_greedy.merge(df_tabu, on="File", how="outer")
    merged = merged.merge(df_bee, on="File", how="outer")

    merged.to_csv(output_file, index=False)

    return merged

def create_latex_table_from_dict(greedy_file, tabu_file, bee_file,
                                 caption="Comparison of Algorithms", label="tab:comparison",
                                 output_file="comparison.tex", value="GrandTotal"):
        concated_dict = {"C1": [], "C2": [], "R1": [], "R2": [], "RC1": [], "RC2": []}
        for key in concated_dict.keys():
            concated_dict[key].append(greedy_file[key])
            concated_dict[key].append(tabu_file[key])
            concated_dict[key].append(bee_file[key])
        for key in concated_dict.keys():
            if concated_dict[key][0] != 0:
                concated_dict[key].append(100*(concated_dict[key][0]-concated_dict[key][1])/concated_dict[key][0])
                concated_dict[key].append(100 * (concated_dict[key][0] - concated_dict[key][2]) / concated_dict[key][0])
            else:
                concated_dict[key].append(100)
                concated_dict[key].append(100)
            concated_dict[key].append(100*(concated_dict[key][2] - concated_dict[key][1]) / concated_dict[key][2])

        with open(output_file, "w") as f:
            f.write("\\begin{table}[ht]\n")
            f.write("\\renewcommand{\\arraystretch}{1.3}\n\n")
            f.write(f"\\caption{{{caption}}}\n")
            f.write(f"\\label{{{label}}}\n")
            f.write("\\centering\n")
            f.write("\\begin{tabular}{l | c | c | c | c | c | c }\n")
            f.write("\\hline\\hline\n")
            f.write("File & Greedy & Tabu & Bee & Tabu & Bee & Tabu \\\\\n")
            f.write("& & & & Greedy & Greedy & Bee \\\\")
            f.write("\\hline \\hline\n")
            for key in concated_dict.keys():
                list = concated_dict[key]
                str = f"{key} & "
                for l in list:
                    str += f"{l:.2f} & "
                str = str[:-2]
                str += "\\\\\n \\hline\n"
                f.write(str)
            f.write("\\hline\\hline\n")
            f.write("\\end{tabular}\n")
            f.write("\\end{table}\n")

def add_improvement_columns(df):
    # Dodanie kolumn z poprawą (wartość procentowa)
    df["tabu_over_greedy"] = (df["GrandTotal_Greedy"] - df["GrandTotal_Tabu"]) / df["GrandTotal_Greedy"]
    df["bee_over_greedy"]  = (df["GrandTotal_Greedy"] - df["GrandTotal_Bee"]) / df["GrandTotal_Greedy"]
    df["tabu_over_bee"]    = (df["GrandTotal_Bee"] - df["GrandTotal_Tabu"])   / df["GrandTotal_Bee"]

    return df


def create_latex_table(df, caption="Comparison of Algorithms", label="tab:comparison",output_file="comparison.tex",value = "GrandTotal"):

    with open(output_file, "w") as f:
        f.write("\\begin{table}[ht]\n")
        f.write("\\renewcommand{\\arraystretch}{1.3}\n\n")
        f.write(f"\\caption{{{caption}}}\n")
        f.write(f"\\label{{{label}}}\n")
        f.write("\\centering\n")
        f.write("\\begin{tabular}{l | c | c | c | c | c | c | c}\n")
        f.write("\\hline\\hline\n")
        f.write("File & Greedy & Tabu & Bee & Tabu & Bee & Tabu \\ \\ \n")
        f.write("& & & & Greedy & Greedy & Bee \\ \\")
        f.write("\\hline \\hline\n")
        for idx, row in df.iterrows():
            f.write(f"{row['File']} & {row[f'{value}_Greedy']} & {row[f'{value}_Tabu']} & {row[f'{value}_Bee']} & "
                    f"{row['tabu_over_greedy']:.4f} & {row['bee_over_greedy']:.4f} & {row['tabu_over_bee']:.4f} \\\\\n")
            f.write("\\hline\n")
        f.write("\\hline\\hline\n")
        f.write("\\end{tabular}\n")
        f.write("\\end{table}\n")

def check_route_correctness(file_path):
    df = pd.read_csv(file_path)
    for idx,row in df.iterrows():
        GTR = row['GTR']
        GTR_splitted = GTR.split(";")
        for i in range(0,100):
            count = GTR_splitted.count(str(i))
            if count!=1 and i !=0:
                print(f"Duplicate or missing stop found in route for file {i}: {row['File']}: {GTR}")


        routes_length = []
        route = []
        for r in GTR_splitted:
            if r != '0':
                route.append(int(r))
            else:
                if len(r) > 0:
                    routes_length.append(len(route))
                    route = []
        print(routes_length)

#
# ### tabela wszystkich plikow  - grandtotal
# result = create_comparison_table(
#     "greedy_results_all.csv",
#     "merged_tabu_results.csv",
#     "merged_bee_results.csv",
#     "comparison.csv"
# )
# df = pd.read_csv("comparison.csv")
# df = add_improvement_columns(add_improvement_columns(df))
# create_latex_table(df, caption="Comparison of Greedy, Tabu, and Bee Algorithms", label="tab:comparison", output_file="comparison.tex")
#
#
#
# #### Srednie dla typow plikow - grandtotal
# create_latex_chart("mean_reults_comparision.tex",
#                    "Porownanie srednich wartosci wynikow algorytmow dla kazdego rodzaju pliku",
#                    "fig:mean_comparision",
#                    "Wartoscc funkcji celu",
#                    "Rodzaj pliku",
#                    [concat_files_mean("greedy_results_all.csv"),concat_files_mean("merged_tabu_results.csv"), concat_files_mean("merged_bee_results.csv")],
#                    ["Greedy", "Tabu", "Bee"],
#                    ("C1","C2","R1","R2","RC1","RC2")
#                    )
# #### Srednie dla typow plikow - wartosci z tabeli vvvv - wykresy
# values = ["TotalPenalty","DrivingCost","AfterHoursCost","CrewCost"]
# for value in values:
#     create_latex_chart(f"mean_{value}_reults_comparision.tex",
#                        f"Porownanie srednich wartosci {value} algorytmow dla kazdego rodzaju pliku",
#                        f"fig:mean_{value}_comparision",
#                        f"Wartoscc funkcji {value}",
#                        "Rodzaj pliku",
#                        [concat_files_mean("greedy_results_all.csv",value=value),concat_files_mean("merged_tabu_results.csv",value=value), concat_files_mean("merged_bee_results.csv",value=value)],
#                        ["Greedy", "Tabu", "Bee"],
#                        ("C1","C2","R1","R2","RC1","RC2")
#                        )
# #### Srednie dla typow plikow - wartosci z tabeli vvvv - tabele
# values = ["TotalPenalty","DrivingCost","AfterHoursCost","CrewCost"]
# for value in values:
#     create_latex_table_from_dict(concat_files_mean("greedy_results_all.csv", value = value),concat_files_mean("merged_tabu_results.csv",value = value), concat_files_mean("merged_bee_results.csv",value = value),
#                        caption=f"Table comparision of {value}",
#                        label = f"tab:comp_{value}",
#                        output_file= f"comparison_{value}.tex",
#                        value = value
#                        )


#Wywolanie podmieniajace wszystkie tabele elementy w latex vv
# concat_files(["bee_results_C1.csv","bee_results_C2.csv","bee_results_R1.csv","bee_results_R2.csv","bee_results_RC1.csv","bee_results_RC2.csv"], output_file="merged_bee_results.csv")
# concat_files(["tabu_results_C1.csv","tabu_results_C2.csv","tabu_results_R1.csv","tabu_results_R2.csv","tabu_results_RC1.csv","tabu_results_RC2.csv"], output_file="merged_tabu_results.csv")


values = ["TotalPenalty","CrewCost"]
for value in values:
    create_latex_table_from_dict(concat_files_mean("greedy_results_all.csv", value = value),concat_files_mean("merged_tabu_results.csv",value = value), concat_files_mean("merged_bee_results.csv",value = value),
                       caption=f"Table comparision of {value}",
                       label = f"tab:comp_{value}",
                       output_file= f"comparison_{value}.tex",
                       value = value
                       )

### tabela wszystkich plikow  - grandtotal
result = create_comparison_table(
    "greedy_results_all.csv",
    "merged_tabu_results.csv",
    "merged_bee_results.csv",
    "comparison.csv"
)
df = pd.read_csv("comparison.csv")
df = add_improvement_columns(add_improvement_columns(df))
create_latex_table(df, caption="Comparison of Greedy, Tabu, and Bee Algorithms", label="tab:comparison", output_file="comparison.tex")



create_latex_chart("mean_reults_comparision.tex",
                   "Porownanie srednich wartosci wynikow algorytmow dla kazdego rodzaju pliku",
                   "fig:mean_comparision",
                   "Wartoscc funkcji celu",
                   "Rodzaj pliku",
                   [concat_files_mean("greedy_results_all.csv"),concat_files_mean("merged_tabu_results.csv"), concat_files_mean("merged_bee_results.csv")],
                   ["Greedy", "Tabu", "Bee"],
                   ("C1","C2","R1","R2","RC1","RC2")
                   )






