import numpy as np
import pandas as pd
import os
from scipy import stats
import matplotlib.pyplot as plt
from mpl_toolkits import mplot3d

DATA_PATH = os.path.dirname(__file__)
no_outlier_file_path = os.path.join(DATA_PATH, "../../Data/interim/D_KeyboardData_6_O.csv")
inverse_data_file_path = os.path.join(DATA_PATH, "../../Data/interim/D_KeyboardData_6_O_Not.csv")


def load_group_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "../../Data/interim/D_KeyboardData_6.csv")
    #csv_path = os.path.join(data_path, "test.csv")
    return pd.read_csv(csv_path)


def calculate_min_max(column_index, array):
    column = array[:, column_index]
    mean_val = int(column.mean())
    std_val = int(column.std())

    max_gen = mean_val + std_val * 4
    min_gen = mean_val - std_val * 4
    return max_gen, min_gen


def distance_between(coord1, coord2):
    distance = np.linalg.norm(coord1 - coord2)
    return distance

def delete_previously_generated_files():
    if os.path.exists(no_outlier_file_path):
        os.remove(no_outlier_file_path)

    if os.path.exists(inverse_data_file_path):
        os.remove(inverse_data_file_path)


def main():
    delete_previously_generated_files()
    group_data = load_group_data(DATA_PATH)
    unique_ids = np.unique(group_data['Id'])
    for combId in unique_ids:
        group_data_of_a_comb = group_data.loc[group_data['Id'] == combId]
        do_thing(group_data_of_a_comb)


def do_thing(group_data):
    all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values

    if len(all_x) <= 1:
        return

    def get_rows_index_of_outliers(x_values):
        z_scores = np.abs(stats.zscore(x_values[:, 1:]))
        z_score_threshold = 3
        indices_above_threshold = np.where(z_scores > z_score_threshold)[0]
        return list(dict.fromkeys(indices_above_threshold))

    rows_with_outliers = get_rows_index_of_outliers(all_x)

    def remove_outliers_from_feature_list(list_of_data):
        for index in sorted(rows_with_outliers, reverse=True):
            list_of_data = np.delete(list_of_data, index, axis=0)

        return list_of_data

    all_x_no_outliers = remove_outliers_from_feature_list(all_x)


    max_ht, min_ht = calculate_min_max(group_data.columns.get_loc("HT"), all_x_no_outliers)
    max_ft, min_ft = calculate_min_max(group_data.columns.get_loc("FT"), all_x_no_outliers)
    max_di1, min_di1 = calculate_min_max(group_data.columns.get_loc('Di1'), all_x_no_outliers)
    max_di2, min_di2 = calculate_min_max(group_data.columns.get_loc('Di2'), all_x_no_outliers)
    max_di3, min_di3 = calculate_min_max(group_data.columns.get_loc('Di3'), all_x_no_outliers)

    ht_data_dif = int((max_ht - min_ht)/5)
    ft_data_dif = int((max_ft - min_ft)/5)
    di1_data_dif = int((max_di1 - min_di1)/5)
    di2_data_dif = int((max_di2 - min_di2)/5)
    di3_data_dif = int((max_di3 - min_di3)/5)

    if ht_data_dif == 0 or ft_data_dif == 0 or di1_data_dif == 0 or di2_data_dif == 0 or di3_data_dif == 0:
        return

    not_data_to_write_to_file = np.empty((0, 4), int)

    for i in range(min_ht, max_ht, ht_data_dif):
        for j in range(min_ft, max_ft, ft_data_dif):
            for k in range(min_di1, max_di1, di1_data_dif):

                too_close = False
                for value in all_x_no_outliers:
                    comb_id = value[0]
                    value = value[1:4]
                    euc_distance = distance_between(value, [i, j, k])#, k, l, m])
                    if euc_distance < 100:
                        too_close = True
                        break

                if not too_close:
                    not_data_to_write_to_file = np.append(not_data_to_write_to_file, np.array([[comb_id, i, j, k]]), axis=0)




    '''x_not_data = not_data_to_write_to_file[:, 1]
    y_not_data = not_data_to_write_to_file[:, 2]
    z_not_data = not_data_to_write_to_file[:, 3]

    x_data = all_x_no_outliers[:, 1]
    y_data = all_x_no_outliers[:, 2]
    z_data = all_x_no_outliers[:, 3]

    ax = plt.axes(projection='3d')
    ax.scatter3D(x_data, y_data, z_data, c='r')
    ax.scatter3D(x_not_data, y_not_data, z_not_data, c='b')'''

    all_x_no_outliers = np.delete(all_x_no_outliers, np.s_[4, 5], axis=1)

    nofp = open(no_outlier_file_path, 'a')
    np.savetxt(nofp, all_x_no_outliers, delimiter=",", fmt='%s', )
    nofp.close()

    f = open(inverse_data_file_path, 'a')
    np.savetxt(f, not_data_to_write_to_file, delimiter=",", fmt='%s',)
    f.close()

if __name__ == '__main__':
    main()




