import os

import numpy as np
import pandas as pd

from generate_inverse_data import remove_outliers_from_feature_list
from data_insights.sort_combinations_by_similarity import sort_by_highest_average
from utils.constants import DEBUG_DATA_PATH
from utils.plot import plot_data


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