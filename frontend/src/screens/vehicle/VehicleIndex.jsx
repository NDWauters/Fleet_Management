import React, {useEffect, useMemo, useState} from "react";
import DataTable from "react-data-table-component";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus} from "@fortawesome/free-solid-svg-icons";

import VehicleColumns from "../../components/vehicle/VehicleColumns";
import {Button, Spinner} from "react-bootstrap";
import ContextHeader from "../../components/ContextHeader";
import {ToastContainer} from "react-toastify";
import {vehicleService} from "../../services/vehicle.service";
import {generalHelper} from "../../helpers/general.helper";
import FilterComponent from "../../components/FilterComponent";
import FilterSelectComponent from "../../components/FilterSelectComponent";

const VehicleIndex = () => {

    const [vehicles, setVehicles] = useState([]);
    const [pending, setPrending] = useState(true);
    const [breadCrumbs, setBreadCrumbs] = useState([]);

    const [filterModel, setFilterModel] = useState('');
    const [filterBrand, setFilterBrand] = useState('');
    const [filterLicensePlate, setFilterLicensePlate] = useState('');
    const [filterVin, setFilterVin] = useState('');
    const [filterDriver, setFilterDriver] = useState('');
    const [filterAvailable, setFilterAvailable] = useState(2);
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);

    //filter all vehicles by search inputs
    const filteredItems = vehicles.filter(
        (el) => {
            let checks = 0;
            if (el.brand && el.brand.toLowerCase().includes(filterBrand.toLowerCase())){
                checks++;
            }
            if (el.model && el.model.toLowerCase().includes(filterModel.toLowerCase())){
                checks++;
            }
            if (el.licensePlate && el.licensePlate.toLowerCase().includes(filterLicensePlate.toLowerCase())){
                checks++;
            }
            if (el.vin && el.vin.toLowerCase().includes(filterVin.toLowerCase())){
                checks++;
            }
            if (el.driver && el.driver.toLowerCase().includes(filterDriver.toLowerCase()) && (filterAvailable == 0 || filterAvailable == 2)){
                checks++;
            }else if (!el.driver && (filterAvailable == 1 || filterAvailable == 2)){
                checks++;
            }

            // if checks is 5, we know all inputs of search match
            if (checks === 5){
                return el;
            }
        }
    );

    useEffect(() => {
        document.title = "Voertuigen";
        vehicleService.getAllVehicles()
            .then((response) => {
                setVehicles(response.data);
                setPrending(false);
                setBreadCrumbs([
                    {link: "/", name: "Home", active: false},
                    {link: "", name: "Voertuigen", active: true},
                ]);
            })
            .catch((err) => generalHelper.handleError(err));
    }, []);

    const subHeaderComponentMemo = useMemo(() => {

        const clearSearch = (filter, setFilter, value) => {
            if (filter) {
                setResetPaginationToggle(!resetPaginationToggle);
                setFilter(value ? value : '');
            }
        }

        return (
            <div className="d-flex flex-row">
                <FilterComponent
                    onFilter={e => setFilterBrand(e.target.value)}
                    onClear={() => clearSearch(filterBrand, setFilterBrand)}
                    filterText={filterBrand}
                    label="Merk"
                />
                <FilterComponent
                    onFilter={e => setFilterModel(e.target.value)}
                    onClear={() => clearSearch(filterModel, setFilterModel)}
                    filterText={filterModel}
                    label="Model"
                />
                <FilterComponent
                    onFilter={e => setFilterLicensePlate(e.target.value)}
                    onClear={() => clearSearch(filterLicensePlate, setFilterLicensePlate)}
                    filterText={filterLicensePlate}
                    label="Nummerplaat"
                />
                <FilterComponent
                    onFilter={e => setFilterVin(e.target.value)}
                    onClear={() => clearSearch(filterVin, setFilterVin)}
                    filterText={filterVin}
                    label="Chassisnummer"
                />
                <FilterComponent
                    onFilter={e => setFilterDriver(e.target.value)}
                    onClear={() => clearSearch(filterDriver, setFilterDriver)}
                    filterText={filterDriver}
                    label="Bestuurder"
                />
                <FilterSelectComponent
                    onChange={(e) => setFilterAvailable(e.target.value)}
                    onClear={() => clearSearch(filterAvailable,setFilterAvailable, 2)}
                    value={filterAvailable}
                    options={[{text: 'Nee', value: 0}, {text: 'Ja', value: 1}, {text: '---', value: 2}]}
                    label="Beschikbaar?"
                />
            </div>
        );
    }, [filterBrand, filterModel, filterLicensePlate, filterVin, filterDriver, filterAvailable, resetPaginationToggle]);

    return (
        <main>
            <ContextHeader breadCrumbs={breadCrumbs} pending={pending}/>
            <div className="col-md-12 datatableSection">
                <Button
                    type="button"
                    className="btn btn-success float-end"
                    href={"/vehicle/create"}
                >
                    <FontAwesomeIcon icon={faPlus}/> Voeg Toe
                </Button>
                <div className="row">
                    <div className="col-md-12">
                        <DataTable
                            className="datatable"
                            title="Voertuigen"
                            pagination
                            highlightOnHover
                            columns={VehicleColumns}
                            data={filteredItems}
                            progressPending={pending}
                            progressComponent={<Spinner animation={"border"}/>}
                            responsive
                            paginationResetDefaultPage={resetPaginationToggle}
                            subHeader
                            subHeaderAlign="center"
                            subHeaderComponent={subHeaderComponentMemo}
                            persistTableHead
                            striped
                        />
                    </div>
                </div>
            </div>
            <ToastContainer
                position="bottom-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
            />
        </main>
    );
};

export default VehicleIndex;
