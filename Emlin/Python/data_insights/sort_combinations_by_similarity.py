import os

import numpy as np
import pandas as pd

from generate_inverse_data import distance_between, remove_outliers_from_feature_list
from utils.constants import DEBUG_DATA_PATH
from utils.plot import get_graph_title


def get_id_of_closest_point(copy_of_data, point, unique_IDs):

    print(len(copy_of_data))

    print("Looking for closest point to ")
    print(point)

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

    print("Closest point is: ")
    print(current_closest_id)

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

def write_list_to_file(ordered_ids, ordered_points):

    with open(os.path.join(DEBUG_DATA_PATH, '../data_insights/ids/ordered_ids.txt'), 'w') as f:
        for item in ordered_ids:
            f.write("%s\n" % item)

    with open(os.path.join(DEBUG_DATA_PATH, '../data_insights/combs/ordered_combs.txt'), 'w') as f:
        for item in ordered_ids:
            f.write("%s\n" % get_graph_title(item))

    with open(os.path.join(DEBUG_DATA_PATH, '../data_insights/points/ordered_points.txt'), 'w') as f:
        for point in ordered_points:
            f.write("%s\n" % point)


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
    test_data_filepath = "../../../Data/interim/D_KeyboardData_test.csv"
    user_data_filepath = "../../../Data/interim/D_KeyboardData_10.csv"

    data = pd.read_csv(os.path.join(DEBUG_DATA_PATH, user_data_filepath))
    unique_ids = np.unique(data['Id'])

    X_clean_data = []
    X_mean = []

    for Id in unique_ids:
        data_of_comb = data.loc[data['Id'] == Id]

        X_of_Id_no_outliers = np.array(remove_outliers_from_feature_list(data_of_comb[['Id', 'HT', 'FT']].values))
        X_mean.append(get_mean_from_data(Id, X_of_Id_no_outliers))

        X_clean_data.append(X_of_Id_no_outliers)

    highest_mean = get_id_with_highest_mean(X_mean)

    current_id = highest_mean
    ordered_ids = [current_id]
    ordered_points = []
    copy_of_X_mean = pd.DataFrame(X_mean)

    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[0]: 'Id'}, inplace=True)
    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[1]: 'HT'}, inplace=True)
    copy_of_X_mean.rename(columns={list(copy_of_X_mean)[2]: 'FT'}, inplace=True)

    while len(copy_of_X_mean) > 1:
        data_where_id = copy_of_X_mean.loc[copy_of_X_mean['Id'] == current_id]
        hold_time = data_where_id[['HT']].values
        flight_time = data_where_id[['FT']].values

        copy_of_X_mean = copy_of_X_mean[copy_of_X_mean.Id != current_id]

        index = np.argwhere(unique_ids == current_id)
        unique_ids = np.delete(unique_ids, index)

        next_closest_id = get_id_of_closest_point(copy_of_X_mean, np.array([hold_time, flight_time]), unique_ids)

        ordered_ids.append(next_closest_id)
        ordered_points.append(np.array([hold_time, flight_time]))

        current_id = next_closest_id

    write_list_to_file(ordered_ids, ordered_points)

    return ordered_ids


if __name__ == '__main__':
    order_id_by_similarity()