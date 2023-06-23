import React from "react";

import {BrowserRouter, Route, Routes} from "react-router-dom";
import {PrivateRoute} from "./components/PrivateRoute";

import VehicleIndex from "./screens/vehicle/VehicleIndex";
import VehicleCreate from "./screens/vehicle/VehicleCreate";
import VehicleEdit from "./screens/vehicle/VehicleEdit";
import VehicleDetails from "./screens/vehicle/VehicleDetails";

import FuelCards from "./screens/fuelcard/FuelCardIndex";
import FuelCardsCreate from "./screens/fuelcard/FuelCardCreate";
import FuelCardsEdit from "./screens/fuelcard/FuelCardEdit";
import FuelCardsDetails from "./screens/fuelcard/FuelCardDetails";

import DriverIndex from "./screens/driver/DriverIndex";
import DriverDetails from "./screens/driver/DriverDetails";
import DriverCreate from "./screens/driver/DriverCreate";
import DriverEdit from "./screens/driver/DriverEdit";

import LoginPage from "./screens/account/LoginPage";

const App = () => {

  return (
        <div className="row">
            <div className="col-md-12">
                <BrowserRouter>
                    <Routes>
                        <Route exact path="/" element={<PrivateRoute/>} />

                        <Route path="/login" element={<LoginPage/>} />

                        <Route path={"/vehicle"} element={<VehicleIndex/>} />
                        <Route path={"/vehicle/create"} element={<VehicleCreate/>} />
                        <Route path={"/vehicle/edit/:id"} element={<VehicleEdit/>} />
                        <Route path={"/vehicle/details/:id"} element={<VehicleDetails/>} />

                        <Route path={"/fuelCard"} element={<FuelCards/>} />
                        <Route path={"/fuelCard/create"} element={<FuelCardsCreate/>} />
                        <Route path={"/fuelCard/edit/:id"} element={<FuelCardsEdit/>} />
                        <Route path={"/fuelCard/details/:id"} element={<FuelCardsDetails/>} />

                        <Route path={"/driver"} element={<DriverIndex/>} />
                        <Route path={"/driver/details/:id"} element={<DriverDetails/>} />
                        <Route path={"/driver/create"} element={<DriverCreate/>} />
                        <Route path={"/driver/edit/:id"} element={<DriverEdit/>} />

                        <Route path={"*"} element={PrivateRoute} />
                    </Routes>
                </BrowserRouter>
            </div>
        </div>
  );
};

export default App;
