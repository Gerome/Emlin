import numpy as np
import pandas as pd
import os
from scipy import stats
import matplotlib.pyplot as plt
from mpl_toolkits import mplot3d
import sys

#DATA_PATH = str(sys.argv[1])
DATA_PATH = os.path.join(os.getenv('APPDATA'), "Emlin")
DEBUG_DATA_PATH = os.path.dirname(__file__)

no_outlier_file_path = os.path.join(DATA_PATH, "D_KeyboardData_O.csv")
inverse_data_file_path = os.path.join(DATA_PATH, "D_KeyboardData_O_Not.csv")


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
        z_score_threshold = 0.4
    else:
        z_score_threshold = -0.001 * number_of_instances + 1.5

    indices_above_threshold = np.where(z_scores > z_score_threshold)[0]
    return list(dict.fromkeys(indices_above_threshold))


def remove_outliers_from_feature_list(list_of_data):
    rows_with_outliers = get_rows_index_of_outliers(list_of_data)

    for index in sorted(rows_with_outliers, reverse=True):
        list_of_data = np.delete(list_of_data, index, axis=0)

    return list_of_data


def do_thing(group_data):
    all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values

    all_x_no_outliers = remove_outliers_from_feature_list(all_x)

    if len(all_x_no_outliers) <= 1:
        return

    min_ht,  max_ht = calculate_min_max(group_data.columns.get_loc("HT"), all_x_no_outliers)
    min_ft,  max_ft = calculate_min_max(group_data.columns.get_loc("FT"), all_x_no_outliers)
    #min_di1, max_di1 = calculate_min_max(group_data.columns.get_loc('Di1'), all_x_no_outliers)
    #min_di2, max_di2 = calculate_min_max(group_data.columns.get_loc('Di2'), all_x_no_outliers)
    #min_di3, max_di3 = calculate_min_max(group_data.columns.get_loc('Di3'), all_x_no_outliers)

    ht_std = all_x_no_outliers[:, group_data.columns.get_loc("HT")].std()
    ft_std = all_x_no_outliers[:, group_data.columns.get_loc("FT")].std()

    average_std = (ht_std+ft_std)/3

    generated_grid_number = 10

    ht_data_dif = int((max_ht - min_ht)/generated_grid_number)
    ft_data_dif = int((max_ft - min_ft)/generated_grid_number)
    #di1_data_dif = int((max_di1 - min_di1)/generated_grid_number)
    #di2_data_dif = int((max_di2 - min_di2)/generated_grid_number)
    #di3_data_dif = int((max_di3 - min_di3)/generated_grid_number)

    if ht_data_dif == 0 or ft_data_dif == 0: # or di1_data_dif == 0 or di2_data_dif == 0 or di3_data_dif == 0:
        return

    not_data_to_write_to_file = np.empty((0, 3), int)

    for i in range(min_ht, max_ht, ht_data_dif):
        for j in range(min_ft, max_ft, ft_data_dif):

                too_close = False
                for value in all_x_no_outliers:
                    comb_id = value[0]
                    value = value[1:3]
                    euc_distance = distance_between(value, [i, j])#, k, l, m])
                    if euc_distance < average_std:
                        too_close = True
                        break

                if not too_close:
                    not_data_to_write_to_file = np.append(not_data_to_write_to_file, np.array([[comb_id, i, j]]), axis=0)

    x_not_data = not_data_to_write_to_file[:, 1]
    y_not_data = not_data_to_write_to_file[:, 2]
    #z_not_data = not_data_to_write_to_file[:, 3]

    x_data = all_x_no_outliers[:, 1]
    y_data = all_x_no_outliers[:, 2]
    #z_data = all_x_no_outliers[:, 3]

    if len(x_data) > 5:
        actual_comb_id = int(comb_id/6000)
        first_ascii = actual_comb_id // 128
        first_char_of_comb = chr(first_ascii)
        second_ascii = int(((actual_comb_id / 128) % 1) * 128)
        second_char_of_comb = chr(second_ascii)

        title = "\'" + first_char_of_comb + '\' to \'' + second_char_of_comb + "\'"
        plot_data(title, x_data, x_not_data, y_data, y_not_data)

    '''
    ax = plt.axes(projection='3d')
    ax.scatter3D(x_data, y_data, z_data, c='r')
    ax.scatter3D(x_not_data, y_not_data, z_not_data, c='b')'''

    all_x_no_outliers = np.delete(all_x_no_outliers, np.s_[3, 4, 5], axis=1)

    nofp = open(no_outlier_file_path, 'a')
    np.savetxt(nofp, all_x_no_outliers, delimiter=",", fmt='%i')
    nofp.close()

    f = open(inverse_data_file_path, 'a')
    np.savetxt(f, not_data_to_write_to_file, delimiter=",", fmt='%i')
    f.close()


def plot_data(title, x_data, x_not_data, y_data, y_not_data):
    plt.title(title)
    plt.xlabel("Hold time")
    plt.ylabel("Flight time")
    plt.plot(y_not_data, x_not_data, 'ro')
    plt.plot(y_data, x_data, 'bx')
    plt.close()


def main():
    delete_previously_generated_files()
    group_data = load_group_data(DATA_PATH)
    unique_ids = np.unique(group_data['Id'])
    for combId in unique_ids:
        group_data_of_a_comb = group_data.loc[group_data['Id'] == combId]
        do_thing(group_data_of_a_comb)


def compare_users():

    user1_data_filepath = "../../Data/interim/D_KeyboardData_1.csv"
    user2_data_filepath = "../../Data/interim/D_KeyboardData_6.csv"

    group_data_user1 = pd.read_csv(os.path.join(DEBUG_DATA_PATH, user1_data_filepath))
    group_data_user2 = pd.read_csv(os.path.join(DEBUG_DATA_PATH, user2_data_filepath))

    unique_ids_user1 = np.unique(group_data_user1['Id'])
    unique_ids_user2 = np.unique(group_data_user2['Id'])

    x = set(unique_ids_user1)
    y = set(unique_ids_user2)

    for shared_comb in sorted(x.intersection(y)):
        group_data_of_a_comb_user1 = group_data_user1.loc[group_data_user1['Id'] == shared_comb]
        group_data_of_a_comb_user2 = group_data_user2.loc[group_data_user2['Id'] == shared_comb]

        all_x_user1 = group_data_of_a_comb_user1[['Id', 'HT', 'FT']].values
        all_x_user2 = group_data_of_a_comb_user2[['Id', 'HT', 'FT']].values

        all_x_user1 = remove_outliers_from_feature_list(all_x_user1)
        all_x_user2 = remove_outliers_from_feature_list(all_x_user2)

        plot_data(str(shared_comb), all_x_user1[:, 1], all_x_user2[:, 1], all_x_user1[:, 2], all_x_user2[:, 2])


if __name__ == '__main__':
    main()




