import pandas as pd

filenames = ["tabu_results_C101.csv","tabu_results_C201.csv","tabu_results_R101.csv","tabu_results_R201.csv","tabu_results_RC101.csv","tabu_results_RC201.csv"]
bee_filenames = ["bee_results_C101.csv","bee_results_C201.csv","bee_results_R101.csv","bee_results_R201.csv","bee_results_RC101.csv","bee_results_RC201.csv"]

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
            for e in data[i]:
                f.write(f"{e}")
            f.write("};\n")
            f.write("\\addlegendentry\n")
            f.write(f"{{{legend_entry[i]}}}\n")
        f.write("\\end{axis}\n")
        f.write("\\end{tikzpicture}\n")
        f.write(f"\\caption\n{{{caption}}}\n")
        f.write(f"\\label{{{label}}}\n")
        f.write("\\end{figure}\n")


def create_chart_mean_goals_per_tabu_size_per_one_file(filename):
    mean_tabu_size = dict()
    df = pd.read_csv(filename)
    for row in df.itertuples():
        mean_tabu_size[row.TabuSize] = []

    for row in df.itertuples():
        mean_tabu_size[row.TabuSize].append(row.GrandTotal)

    for element in mean_tabu_size:
        mean_tabu_size[element] = sum(mean_tabu_size[element]) / len(mean_tabu_size[element])
    str = ""
    for element in mean_tabu_size:
        str+=f"({element},{mean_tabu_size[element]})"

    return str


def create_chart_mean_goals_per_tabu_size_per_all_files():
    mean_tabu_size = dict()
    for filename in filenames:
        df = pd.read_csv(filename)
        for row in df.itertuples():
            if row.TabuSize not in mean_tabu_size:
                mean_tabu_size[row.TabuSize] = []


        for row in df.itertuples():
            mean_tabu_size[row.TabuSize].append(row.GrandTotal)
    for element in mean_tabu_size:
        mean_tabu_size[element] = sum(mean_tabu_size[element]) / len(mean_tabu_size[element])
    str=""
    for element in mean_tabu_size:
        str+=f"({element},{mean_tabu_size[element]})"
    return str

def create_chart_mean_goals_per_move_type_per_file(filename):
    mean_move_type = dict()
    df = pd.read_csv(filename)
    for row in df.itertuples():
        if row.Operator not in mean_move_type:
            mean_move_type[row.Operator] = []

    for row in df.itertuples():
        mean_move_type[row.Operator].append(row.GrandTotal)

    for element in mean_move_type:
        mean_move_type[element] = sum(mean_move_type[element]) / len(mean_move_type[element])
    str = ""
    for element in mean_move_type:
        str+=f"({element},{mean_move_type[element]})"

    return str

def create_chart_mean_goals_per_move_type_per_all_files():
    mean_move_type = dict()
    for filename in filenames:
        df = pd.read_csv(filename)
        for row in df.itertuples():
            if row.Operator not in mean_move_type:
                mean_move_type[row.Operator] = []


        for row in df.itertuples():
            mean_move_type[row.Operator].append(row.GrandTotal)
    for element in mean_move_type:
        mean_move_type[element] = sum(mean_move_type[element]) / len(mean_move_type[element])
    str=""
    for element in mean_move_type:
        str+=f"({element},{mean_move_type[element]})"
    return str

def create_chart_mean_goals_per_bee_number_per_one_file(filename):
    mean_bee_number = dict()
    df = pd.read_csv(filename)
    for row in df.itertuples():
        if row.BeesNumber not in mean_bee_number:
            mean_bee_number[row.BeesNumber] = []

    for row in df.itertuples():
        mean_bee_number[row.BeesNumber].append(row.GrandTotal)

    for element in mean_bee_number:
        mean_bee_number[element] = sum(mean_bee_number[element]) / len(mean_bee_number[element])
    str = ""
    for element in mean_bee_number:
        str+=f"({element},{mean_bee_number[element]})"

    return str
def create_chart_mean_goals_per_bee_number_per_all_files():
    mean_bee_number = dict()
    for filename in bee_filenames:
        df = pd.read_csv(filename)
        for row in df.itertuples():
            if row.BeesNumber not in mean_bee_number:
                mean_bee_number[row.BeesNumber] = []


        for row in df.itertuples():
            mean_bee_number[row.BeesNumber].append(row.GrandTotal)
    for element in mean_bee_number:
        mean_bee_number[element] = sum(mean_bee_number[element]) / len(mean_bee_number[element])
    str=""
    for element in mean_bee_number:
        str+=f"({element},{mean_bee_number[element]})"
    return str

def create_chart_mean_goals_per_limit_per_one_file(filename):
    mean_limit = dict()
    df = pd.read_csv(filename)
    for row in df.itertuples():
        if row.Limit not in mean_limit:
            mean_limit[row.Limit] = []
    for row in df.itertuples():
        mean_limit[row.Limit].append(row.GrandTotal)
    for element in mean_limit:
        mean_limit[element] = sum(mean_limit[element]) / len(mean_limit[element])
    str = ""
    for element in mean_limit:
        str+=f"({element},{mean_limit[element]})"
    return str

def create_chart_mean_goals_per_limit_per_all_files():
    mean_limit = dict()
    for filename in bee_filenames:
        df = pd.read_csv(filename)
        for row in df.itertuples():
            if row.Limit not in mean_limit:
                mean_limit[row.Limit] = []
        for row in df.itertuples():
            mean_limit[row.Limit].append(row.GrandTotal)
    for element in mean_limit:
        mean_limit[element] = sum(mean_limit[element]) / len(mean_limit[element])
    str=""
    for element in mean_limit:
        str+=f"({element},{mean_limit[element]})"
    return str

def create_chart_mean_goals_per_bee_operator_per_one_file(filename):
    mean_bee_operator = dict()
    df = pd.read_csv(filename)
    for row in df.itertuples():
        if row.Operator not in mean_bee_operator:
            mean_bee_operator[row.Operator] = []
    for row in df.itertuples():
        mean_bee_operator[row.Operator].append(row.GrandTotal)
    for element in mean_bee_operator:
        mean_bee_operator[element] = sum(mean_bee_operator[element]) / len(mean_bee_operator[element])
    str = ""
    for element in mean_bee_operator:
        str+=f"({element},{mean_bee_operator[element]})"
    return str

def create_chart_mean_goals_per_bee_operator_per_all_files():
    mean_bee_operator = dict()
    for filename in bee_filenames:
        df = pd.read_csv(filename)
        for row in df.itertuples():
            if row.Operator not in mean_bee_operator:
                mean_bee_operator[row.Operator] = []
        for row in df.itertuples():
            mean_bee_operator[row.Operator].append(row.GrandTotal)
    for element in mean_bee_operator:
        mean_bee_operator[element] = sum(mean_bee_operator[element]) / len(mean_bee_operator[element])
    str=""
    for element in mean_bee_operator:
        str+=f"({element},{mean_bee_operator[element]})"
    return str

create_latex_chart("mean_goals_per_tabu_size_all_instances.tex",
                     "Srednia wartosc funkcji celu w zaleznosci od rozmiaru tabu dla wszystkich instancji",
                     "fig:mean_goals_per_tabu_size_all_instances",
                     "Rozmiar tabu",
                     "Srednia wartosc funkcji celu",
                     [[create_chart_mean_goals_per_tabu_size_per_all_files()]],
                     ["Srednia wartosc funkcji celu"])

create_latex_chart("mean_goals_per_tabu_size_per_instance.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od rozmiaru tabu dla poszczegolnych instancji",
                        "fig:mean_goals_per_tabu_size_per_instance",
                        "Rozmiar tabu",
                        "Srednia wartosc funkcji celu",
                        [ [create_chart_mean_goals_per_tabu_size_per_one_file(filename)] for filename in filenames],
                        [filename.replace("tabu_results_","").replace(".csv","") for filename in filenames])

create_latex_chart("mean_goals_per_move_type_all_instances.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od typu ruchu dla wszystkich instancji",
                        "fig:mean_goals_per_move_type_all_instances",
                        "Typ ruchu",
                        "Srednia wartosc funkcji celu",
                        [[create_chart_mean_goals_per_move_type_per_all_files()]],
                        ["Srednia wartosc funkcji celu"],

                        symbolic_x_coords=("SwapMove","InsertMove","ReverseMove","TwoOptMove")
                   )


create_latex_chart("mean_goals_per_move_type_per_instance.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od typu ruchu dla poszczegolnych instancji",
                        "fig:mean_goals_per_move_type_per_instance",
                        "Typ ruchu",
                        "Srednia wartosc funkcji celu",
                        [ [create_chart_mean_goals_per_move_type_per_file(filename)] for filename in filenames],
                        [filename.replace("tabu_results_","").replace(".csv","") for filename in filenames],
                        symbolic_x_coords=("SwapMove","InsertMove","ReverseMove","TwoOptMove")
                   )



create_latex_chart("mean_goals_per_bee_number_all_instances.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od liczby pszczol dla wszystkich instancji",
                        "fig:mean_goals_per_bee_number_all_instances",
                        "Liczba pszczol",
                        "Srednia wartosc funkcji celu",
                        [[create_chart_mean_goals_per_bee_number_per_all_files()]],
                        ["Srednia wartosc funkcji celu"],
                        symbolic_x_coords=("25","50","75")
                   )
create_latex_chart("mean_goals_per_bee_number_per_instance.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od liczby pszczol dla poszczegolnych instancji",
                        "fig:mean_goals_per_bee_number_per_instance",
                        "Liczba pszczol",
                        "Srednia wartosc funkcji celu",
                        [ [create_chart_mean_goals_per_bee_number_per_one_file(filename)] for filename in bee_filenames],
                        [filename.replace("bee_results_","").replace(".csv","") for filename in bee_filenames],
                        symbolic_x_coords=("25","50","75")
                   )

create_latex_chart("mean_goals_per_limit_all_instances.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od limitu dla wszystkich instancji",
                        "fig:mean_goals_per_limit_all_instances",
                        "Limit",
                        "Srednia wartosc funkcji celu",
                        [[create_chart_mean_goals_per_limit_per_all_files()]],
                        ["Srednia wartosc funkcji celu"],
                        symbolic_x_coords=("20","40")
                   )
create_latex_chart("mean_goals_per_limit_per_instance.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od limitu dla poszczegolnych instancji",
                        "fig:mean_goals_per_limit_per_instance",
                        "Limit",
                        "Srednia wartosc funkcji celu",
                        [ [create_chart_mean_goals_per_limit_per_one_file(filename)] for filename in bee_filenames],
                        [filename.replace("bee_results_","").replace(".csv","") for filename in bee_filenames],
                        symbolic_x_coords=("20","40")
                   )

create_latex_chart("mean_goals_per_bee_operator_all_instances.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od typu operatora dla wszystkich instancji",
                        "fig:mean_goals_per_bee_operator_all_instances",
                        "Typ operatora",
                        "Srednia wartosc funkcji celu",
                        [[create_chart_mean_goals_per_bee_operator_per_all_files()]],
                        ["Srednia wartosc funkcji celu"],
                        symbolic_x_coords=("SwapMove","InsertMove","ReverseMove","TwoOptMove")
                   )

create_latex_chart("mean_goals_per_bee_operator_per_instance.tex",
                        "Srednia wartosc funkcji celu w zaleznosci od typu operatora dla poszczegolnych instancji",
                        "fig:mean_goals_per_bee_operator_per_instance",
                        "Typ operatora",
                        "Srednia wartosc funkcji celu",
                        [ [create_chart_mean_goals_per_bee_operator_per_one_file(filename)] for filename in bee_filenames],
                        [filename.replace("bee_results_","").replace(".csv","") for filename in bee_filenames],
                        symbolic_x_coords=("SwapMove","InsertMove","ReverseMove","TwoOptMove")
                   )

tab = [0,2,65,2,7,0,42,63,40,44,46,45,0,52,50,48,0,68,51,31,37,34,47,0,90,86,94,92,96,95,0,28,23,66,69,49,0,20,24,8,10,11,9,14,0,5,1,75,0,26,22,21,0,88,89,99,98,91,0,0,67,41,62,74,56,60,0,64,0,43,25,27,29,30,61,0,78,70,73,0,32,33,19,15,0,55,53,58,0,13,18,35,93,97,100,0,36,39,72,0,87,85,84,0,79,80,77,82,83,0,17,12,16,0,81,76,71,38,0,57,54,59,0,6,4,3,0]
for i in range(0,100):
    if tab.count(i)!=1:
        print(f"Element {i} wystepuje {tab.count(i)} razy")