import numpy as np
import pandas as pd
import os
from scipy import stats
from utils.plot import plot_data, get_graph_title

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


def generate_inverse_data_for_comb(group_data):
    all_x = group_data[['Id', 'HT', 'FT']].values

    all_x_no_outliers = remove_outliers_from_feature_list(all_x)

    if len(all_x_no_outliers) <= 1:
        return

    min_ht,  max_ht = calculate_min_max(group_data.columns.get_loc("HT"), all_x_no_outliers)
    min_ft,  max_ft = calculate_min_max(group_data.columns.get_loc("FT"), all_x_no_outliers)

    ht_std = all_x_no_outliers[:, group_data.columns.get_loc("HT")].std()
    ft_std = all_x_no_outliers[:, group_data.columns.get_loc("FT")].std()

    average_std = (ht_std+ft_std)/3

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
        pass
        #plot_data(comb_id, all_data)

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


def main():
    delete_previously_generated_files()
    group_data = load_group_data(DATA_PATH)
    unique_ids = np.unique(group_data['Id'])
    for combId in unique_ids:
        group_data_of_a_comb = group_data.loc[group_data['Id'] == combId]
        generate_inverse_data_for_comb(group_data_of_a_comb)


def get_id_of_closest_point(copy_of_data, point):
    unique_IDs = np.unique(copy_of_data['Id'])

    current_closest_id = 0
    current_closest_mean = 9999

    for id in unique_IDs:
        data_where_id = copy_of_data.loc[copy_of_data['Id'] == id]
        hold_time_mean = data_where_id[['HT']].values
        flight_time_mean = data_where_id[['FT']].values

        euc_distance = distance_between(np.array([[hold_time_mean, flight_time_mean]]), point)

        if euc_distance < current_closest_mean:
            current_closest_id = id
            current_closest_mean = euc_distance

    return current_closest_id



def sort_by_highest_average(data, unique_IDs):

    start_id = get_id_with_highest_mean(data, unique_IDs)
    copy_of_data = data
    current_id = start_id

    ordered_ids = [start_id]

    while len(copy_of_data) > 1:
        data_where_id = copy_of_data.loc[copy_of_data['Id'] == current_id]
        hold_time_mean = data_where_id[['HT']].values.mean()
        flight_time_mean = data_where_id[['FT']].values.mean()

        copy_of_data = copy_of_data[copy_of_data.Id != current_id]

        next_closest_id = get_id_of_closest_point(copy_of_data, np.array([[hold_time_mean, flight_time_mean]]))
        ordered_ids.append(next_closest_id)

        current_id = next_closest_id

    return ordered_ids

def get_id_with_highest_mean(data):

    current_highest_id = 0
    current_highest_mean = 0

    for row in data:
        euc_dist = np.sqrt(row[1] * row[1] + row[2] * row[2])

        if euc_dist > current_highest_mean:
            current_highest_mean = euc_dist
            current_highest_id = row[0]

    return current_highest_id


def get_mean_from_data(Id, X_of_Id_no_outliers):
    ht_mean = int(X_of_Id_no_outliers[:, 1].mean())
    ft_mean = int(X_of_Id_no_outliers[:, 2].mean())
    return [Id, ht_mean, ft_mean]


def order_id_by_similarity():
    test_data_filepath = "../../Data/interim/D_KeyboardData_test.csv"
    user_data_filepath = "../../Data/interim/D_KeyboardData.csv"

    data = pd.read_csv(os.path.join(DEBUG_DATA_PATH, user_data_filepath))
    unique_ids = np.unique(data['Id'])

    X_clean_data = []
    X_mean = []

    for Id in unique_ids:
        data_of_comb = data.loc[data['Id'] == Id]

        X_of_Id_no_outliers = np.array(remove_outliers_from_feature_list(data_of_comb[['Id', 'HT', 'FT']].values))
        X_mean.append(get_mean_from_data(Id/6000, X_of_Id_no_outliers))

        X_clean_data.append(X_of_Id_no_outliers)

    highest_mean = get_id_with_highest_mean(X_mean)

    current_id = highest_mean
    ordered_ids = [current_id]
    copy_of_X_mean = pd.DataFrame(X_mean)

    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[0]: 'Id'}, inplace=True)
    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[1]: 'HT'}, inplace=True)
    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[2]: 'FT'}, inplace=True)

    while len(copy_of_X_mean) > 1:
        data_where_id = copy_of_X_mean.loc[copy_of_X_mean['Id'] == current_id]
        hold_time = data_where_id[['HT']].values
        flight_time = data_where_id[['FT']].values

        copy_of_X_mean = copy_of_X_mean[copy_of_X_mean.Id != current_id]

        next_closest_id = get_id_of_closest_point(copy_of_X_mean, np.array([[hold_time, flight_time]]))
        ordered_ids.append(next_closest_id)
        whatever = get_graph_title(current_id)
        current_id = next_closest_id

    return ordered_ids

def compare_users():
    user1_data_filepath = "../../Data/interim/D_KeyboardData_1.csv"
    user2_data_filepath = "../../Data/interim/D_KeyboardData_6.csv"
    user3_data_filepath = "../../Data/interim/D_KeyboardData_10.csv"
    user_test_data_filepath = "../../Data/interim/D_KeyboardData_test.csv"

    user_data_filepaths = [user1_data_filepath, user1_data_filepath]

    data_users = []

    for path in user_data_filepaths:
        data_users.append(pd.read_csv(os.path.join(DEBUG_DATA_PATH, path)))


    unique_IDs_list = []

    for data in data_users:
        unique_IDs = np.unique(data['Id'])
        unique_IDs_list.append(unique_IDs)
        sort_by_highest_average(data, unique_IDs)

    x = set(unique_IDs_list[0])
    y = set(unique_IDs_list[1])

    for shared_comb in x.intersection(y):

        data_of_comb = []

        for data in data_users:
            data_of_comb.append(data.loc[data['Id'] == shared_comb])

        all_x_data = []

        for user in data_of_comb:
            x_data_no_outlier = remove_outliers_from_feature_list(user[['Id', 'HT', 'FT']].values)
            all_x_data.append(x_data_no_outlier)


        plot_data(shared_comb, all_x_data)


if __name__ == '__main__':
    main()

