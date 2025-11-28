import os

def extract_n_records(n: int, filename: str):
    basename = os.path.basename(filename)

    output_dir = f"pliki/{n} lokacji"
    os.makedirs(output_dir, exist_ok=True)

    output_path = os.path.join(output_dir, basename)

    with open(filename, "r", encoding="utf-8") as f:
        lines = f.readlines()

    header = lines[0]
    data_lines = lines[1:n+1]

    with open(output_path, "w", encoding="utf-8") as f:
        f.write(header)
        f.writelines(data_lines)

    print(f"Zapisano {n} rekordów do: {output_path}")


filenames = ["pliki//100 lokacji//C101.txt","pliki//100 lokacji//C201.txt","pliki//100 lokacji//R101.txt","pliki//100 lokacji//R201.txt","pliki//100 lokacji//RC101.txt","pliki//100 lokacji//RC201.txt"]

filenames = [
    "100 lokacji/C101.txt", "100 lokacji/C102.txt", "100 lokacji/C103.txt",
    "100 lokacji/C104.txt", "100 lokacji/C105.txt", "100 lokacji/C106.txt", "100 lokacji/C107.txt",
    "100 lokacji/C108.txt", "100 lokacji/C109.txt", "100 lokacji/C201.txt", "100 lokacji/C202.txt",
    "100 lokacji/C203.txt", "100 lokacji/C204.txt", "100 lokacji/C205.txt", "100 lokacji/C206.txt",
    "100 lokacji/C207.txt", "100 lokacji/C208.txt", "100 lokacji/R101.txt", "100 lokacji/R102.txt",
    "100 lokacji/R103.txt", "100 lokacji/R104.txt", "100 lokacji/R105.txt", "100 lokacji/R106.txt",
    "100 lokacji/R107.txt", "100 lokacji/R108.txt", "100 lokacji/R109.txt", "100 lokacji/R110.txt",
    "100 lokacji/R111.txt", "100 lokacji/R112.txt", "100 lokacji/R201.txt", "100 lokacji/R202.txt",
    "100 lokacji/R203.txt", "100 lokacji/R204.txt", "100 lokacji/R205.txt", "100 lokacji/R206.txt",
    "100 lokacji/R207.txt", "100 lokacji/R208.txt", "100 lokacji/R209.txt", "100 lokacji/R210.txt",
    "100 lokacji/R211.txt", "100 lokacji/RC101.txt", "100 lokacji/RC102.txt", "100 lokacji/RC103.txt",
    "100 lokacji/RC104.txt", "100 lokacji/RC105.txt", "100 lokacji/RC106.txt", "100 lokacji/RC107.txt",
    "100 lokacji/RC108.txt", "100 lokacji/RC201.txt", "100 lokacji/RC202.txt", "100 lokacji/RC203.txt",
    "100 lokacji/RC204.txt", "100 lokacji/RC205.txt", "100 lokacji/RC206.txt", "100 lokacji/RC207.txt", "100 lokacji/RC208.txt"]
ns = [9,10,11,12,13,14,15,16]

for n in ns:
    for filename in filenames:
        extract_n_records(n,"pliki//"+filename)