import React, {useEffect, useMemo, useState} from "react";
import "../../styles/driver/index.css";
import DataTable from "react-data-table-component";
import DriverColumns from "../../components/driver/DriverColumns";
import {Button, Spinner} from "react-bootstrap";
import ContextHeader from "../../components/ContextHeader";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus} from "@fortawesome/free-solid-svg-icons";
import {ToastContainer} from "react-toastify";
import {driverService} from "../../services/driver.service";
import {generalHelper} from "../../helpers/general.helper";
import FilterComponent from "../../components/FilterComponent";
import FilterSelectComponent from "../../components/FilterSelectComponent";

const DriverIndex = () => {

    const [drivers, setDrivers] = useState([]);
    const [pending, setPending] = useState(true);
    const [breadCrumbs, setBreadCrumbs] = useState([]);

    const [filterName, setFilterName] = useState('');
    const [filterPlace, setFilterPlace] = useState('');
    const [filterVehicle, setFilterVehicle] = useState('');
    const [filterFuelCard, setFilterFuelCard] = useState('');
    const [filterHasVehicle, setFilterHasVehicle] = useState(2);
    const [filterHasFuelCard, setFilterHasFuelCard] = useState(2);
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);

    //filter all vehicles by search inputs
    const filteredItems = drivers.filter(
        (el) => {
            let checks = 0;
            if (el.name && el.name.toLowerCase().includes(filterName.toLowerCase())){
                checks++;
            }
            if (el.place && el.place.toLowerCase().includes(filterPlace.toLowerCase())){
                checks++;
            }
            if (el.licensePlate && el.licensePlate.toLowerCase().includes(filterVehicle.toLowerCase()) && (filterHasVehicle == 1 || filterHasVehicle == 2)){
                checks++;
            }else if(!el.licensePlate && (filterHasVehicle == 0 || filterHasVehicle == 2)){
                checks++;
            }
            if (el.cardNumber && el.cardNumber.toLowerCase().includes(filterFuelCard.toLowerCase()) && (filterHasFuelCard == 1 || filterHasFuelCard == 2)){
                checks++;
            }else if(!el.cardNumber && (filterHasFuelCard == 0 || filterHasFuelCard == 2)){
                checks++;
            }

            // if checks is 4, we know all inputs of search match
            if (checks === 4){
                return el;
            }
        }
    );

    useEffect(() => {
        document.title = "Bestuurders";
        driverService.getAllDrivers()
            .then((response) => {
                setDrivers(response.data);
                setPending(false);
                setBreadCrumbs([
                    {link: "/", name: "Home", active: false},
                    {link: "", name: "Bestuurders", active: true},
                ]);
            })
            .catch((err) => generalHelper.handleError(err));
    }, []);

    const subHeaderComponentMemo = useMemo(() => {

        const clearSearch = (filter, setFilter, value) => {
            console.log("check");
            if (filter) {
                setResetPaginationToggle(!resetPaginationToggle);
                setFilter(value ? value : '');
            }
        }

        return (
            <div className="d-flex flex-row">
                <FilterComponent
                    onFilter={e => setFilterName(e.target.value)}
                    onClear={() => clearSearch(filterName, setFilterName)}
                    filterText={filterName}
                    label="Naam"
                />
                <FilterComponent
                    onFilter={e => setFilterPlace(e.target.value)}
                    onClear={() => clearSearch(filterPlace, setFilterPlace)}
                    filterText={filterPlace}
                    label="Plaats"
                />
                <FilterComponent
                    onFilter={e => setFilterVehicle(e.target.value)}
                    onClear={() => clearSearch(filterVehicle, setFilterVehicle)}
                    filterText={filterVehicle}
                    label="Voertuig"
                />
                <FilterComponent
                    onFilter={e => setFilterFuelCard(e.target.value)}
                    onClear={() => clearSearch(filterFuelCard, setFilterFuelCard)}
                    filterText={filterFuelCard}
                    label="Tankkaart"
                />
                <FilterSelectComponent
                    onChange={(e) => setFilterHasVehicle(e.target.value)}
                    onClear={() => clearSearch(filterHasVehicle, setFilterHasVehicle,2)}
                    value={filterHasVehicle}
                    options={[{text: 'Nee', value: 0}, {text: 'Ja', value: 1}, {text: '---', value: 2}]}
                    label="Voertuig?"
                />
                <FilterSelectComponent
                    onChange={(e) => setFilterHasFuelCard(e.target.value)}
                    onClear={() => clearSearch(filterHasFuelCard, setFilterHasFuelCard,2)}
                    value={filterHasFuelCard}
                    options={[{text: 'Nee', value: 0}, {text: 'Ja', value: 1}, {text: '---', value: 2}]}
                    label="Tankkaart?"
                />
            </div>
        );
    }, [filterName, filterPlace, filterVehicle, filterFuelCard,filterHasVehicle, filterHasFuelCard, resetPaginationToggle]);

    return (
        <main>
            <ContextHeader breadCrumbs={breadCrumbs}/>
            <div className="col-md-12 datatableSection">
                <Button
                    type="button"
                    className="btn btn-success float-end"
                    href={"/driver/create"}
                >
                    <FontAwesomeIcon icon={faPlus}/> Voeg Toe
                </Button>
                <div className="row">
                    <div className="col-md-12">
                        <DataTable
                            className="datatable"
                            title="Bestuurders"
                            pagination
                            highlightOnHover
                            columns={DriverColumns}
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
}

export default DriverIndex;
