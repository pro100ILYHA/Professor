cmake_minimum_required(VERSION 3.20)
project(KapitalStaff LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_AUTOMOC ON)

find_package(Qt6 REQUIRED COMPONENTS Widgets Sql)

qt_add_executable(KapitalStaff
    src/main.cpp
    src/MainWindow.h
    src/MainWindow.cpp
    src/LoginDialog.h
    src/LoginDialog.cpp
    src/EmployeeEditDialog.h
    src/EmployeeEditDialog.cpp
    src/HistoryWidget.h
    src/HistoryWidget.cpp
)

target_link_libraries(KapitalStaff PRIVATE Qt6::Widgets Qt6::Sql)
