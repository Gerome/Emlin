import numpy as np
import pandas as pd
import os
from scipy import stats
from sklearn import preprocessing
from sklearn.model_selection import train_test_split

from utils.constants import *
from utils.plot import plot_data


def load_group_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "D_KeyboardData.csv")
    return pd.read_csv(csv_path)


def calculate_min_max(column_index, array):
    column = array[:, column_index]
    max_val = int(column.max())
    min_val = int(column.min())
    std_val = int(column.std())

    max_gen = max_val + std_val * 3
    min_gen = min_val - std_val * 3
    return min_gen, max_gen


def distance_between(coord1, coord2):
    distance = np.linalg.norm(coord1 - coord2)
    return distance


def delete_previously_generated_files():
    if os.path.exists(no_outlier_file_path):
        os.remove(no_outlier_file_path)

    if os.path.exists(inverse_data_file_path):
        os.remove(inverse_data_file_path)


def get_rows_index_of_outliers(x_values):
    z_scores = np.abs(stats.zscore(x_values[:, 1:]))
    number_of_instances = len(x_values)

    if number_of_instances > 1000:
        z_score_threshold = 0.3
    else:
        z_score_threshold = -0.001 * number_of_instances + 1.5

    indices_above_threshold = np.where(z_scores > z_score_threshold)[0]
    return list(dict.fromkeys(indices_above_threshold))


def remove_outliers_from_feature_list(list_of_data):
    rows_with_outliers = get_rows_index_of_outliers(list_of_data)

    for index in sorted(rows_with_outliers, reverse=True):
        list_of_data = np.delete(list_of_data, index, axis=0)

    return list_of_data


def generate_inverse_data_for_comb(group_data, testing_mode):

    all_x = group_data[['Id', 'HT', 'FT']].values

    all_x_no_outliers = remove_outliers_from_feature_list(all_x)

    #if testing_mode and all_x_no_outliers.size > 4:
    #    all_x_no_outliers_test, all_x_no_outliers = train_test_split(all_x_no_outliers, shuffle=True, train_size=0.10)
    #    test_set = open(test_file_path, 'a')
    #    np.savetxt(test_set, all_x_no_outliers_test, delimiter=",", fmt='%i')
    #    test_set.close()

    if len(all_x_no_outliers) <= 1:
        return

    min_ht,  max_ht = calculate_min_max(group_data.columns.get_loc("HT"), all_x_no_outliers)
    min_ft,  max_ft = calculate_min_max(group_data.columns.get_loc("FT"), all_x_no_outliers)

    ht_std = all_x_no_outliers[:, group_data.columns.get_loc("HT")].std()
    ft_std = all_x_no_outliers[:, group_data.columns.get_loc("FT")].std()

    average_std = (ht_std + ft_std)/3

    generated_grid_number = 10

    ht_data_dif = int((max_ht - min_ht)/generated_grid_number)
    ft_data_dif = int((max_ft - min_ft)/generated_grid_number)

    if ht_data_dif == 0 or ft_data_dif == 0:
        return

    not_data_to_write_to_file = np.empty((0, 3), int)

    for i in range(min_ht, max_ht, ht_data_dif):
        for j in range(min_ft, max_ft, ft_data_dif):

                too_close = False
                for value in all_x_no_outliers:
                    comb_id = value[0]
                    value = value[1:3]
                    euc_distance = distance_between(value, [i, j])
                    if euc_distance < average_std:
                        too_close = True
                        break

                if not too_close:
                    not_data_to_write_to_file = np.append(not_data_to_write_to_file, np.array([[comb_id, i, j]]), axis=0)

    x_not_data = not_data_to_write_to_file[:, 1]
    y_not_data = not_data_to_write_to_file[:, 2]

    x_data = all_x_no_outliers[:, 1]
    y_data = all_x_no_outliers[:, 2]

    all_data = [x_data, y_data]

    if len(x_data) > 5:
        #plot_data(comb_id, all_data)
        pass

    nofp = open(no_outlier_file_path, 'a')
    np.savetxt(nofp, all_x_no_outliers, delimiter=",", fmt='%i')
    nofp.close()

    f = open(inverse_data_file_path, 'a')
    np.savetxt(f, not_data_to_write_to_file, delimiter=",", fmt='%i')
    f.close()


def main():
    delete_previously_generated_files()
    group_data = load_group_data(DATA_PATH)
    unique_ids = np.unique(group_data['Id'])
    for combId in unique_ids:
        group_data_of_a_comb = group_data.loc[group_data['Id'] == combId]
        generate_inverse_data_for_comb(group_data_of_a_comb, False)


if __name__ == '__main__':
    main()

