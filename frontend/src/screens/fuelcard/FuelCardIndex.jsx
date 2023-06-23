import React, {useEffect, useMemo, useState} from "react";
import DataTable from "react-data-table-component";
import { Button, Spinner } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";

import FuelcardColumns from "../../components/fuelcard/FuelCardColums";
import ContextHeader from "../../components/ContextHeader";
import {generalHelper} from "../../helpers/general.helper";
import {fuelCardService} from "../../services/fuelCard.service";
import FilterComponent from "../../components/FilterComponent";
import FilterSelectComponent from "../../components/FilterSelectComponent";
import moment from "moment";

const Index = () => {

  const [fuelCard, setFuelcard] = useState([]);
  const [pending, setPending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);

    const [filterNumber, setFilterNumber] = useState('');
    const [filterExpirationDate, setFilterExpirationDate] = useState('');
    const [filterDriver, setFilterDriver] = useState('');
    const [filterActive, setFilterActive] = useState(2);
    const [filterFuelType, setFilterFuelType] = useState('');
    const [filterAvailable, setFilterAvailable] = useState(2);
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);

    //filter all vehicles by search inputs
    const filteredItems = fuelCard.filter(
        (el) => {
            let checks = 0;
            if (el.cardNumber && el.cardNumber.toLowerCase().includes(filterNumber.toLowerCase())){
                checks++;
            }
            if (el.expirationDate && moment(el.expirationDate).format('DD-MM-YYYY').includes(filterExpirationDate)){
                checks++;
            }
            if (el.driver && el.driver.toLowerCase().includes(filterDriver.toLowerCase()) && (filterAvailable == 0 || filterAvailable == 2)){
                checks++;
            }
            else if (!el.driver && (filterAvailable == 1 || filterAvailable == 2)){
                checks++;
            }
            if (el.isdisabled && (filterActive == 0 || filterActive == 2)){
                checks++;
            }else if (!el.isdisabled && (filterActive == 1 || filterActive == 2)) {
                checks++;
            }
            if (el.fuelType && el.fuelType.toLowerCase().includes(filterFuelType.toLowerCase())){
                checks++;
            }
            // if checks is 6, we know all inputs of search match
            if (checks === 5){
                return el;
            }
        }
    );

  useEffect(() => {
    document.title = "Tankkaarten";
    fuelCardService.getAllFuelCards()
      .then((response) => {
        setFuelcard(response.data);
        setPending(false);
        setBreadCrumbs([
          { link: "/", name: "Home", active: false },
          { link: "", name: "Tankkaarten", active: true },
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
                    onFilter={e => setFilterNumber(e.target.value)}
                    onClear={() => clearSearch(filterNumber, setFilterNumber)}
                    filterText={filterNumber}
                    label="Kaartnummer"
                />
                <FilterComponent
                    onFilter={e => setFilterExpirationDate(e.target.value)}
                    onClear={() => clearSearch(filterExpirationDate, setFilterExpirationDate)}
                    filterText={filterExpirationDate}
                    label="Vervaldatum"
                />
                <FilterComponent
                    onFilter={e => setFilterDriver(e.target.value)}
                    onClear={() => clearSearch(filterDriver, setFilterDriver)}
                    filterText={filterDriver}
                    label="Bestuurder"
                />
                <FilterSelectComponent
                    onChange={(e) => setFilterActive(e.target.value)}
                    onClear={() => clearSearch(filterActive,setFilterActive, 2)}
                    value={filterActive}
                    options={[{text: 'Nee', value: 0}, {text: 'Ja', value: 1}, {text: '---', value: 2}]}
                    label="Actief?"
                />
                <FilterComponent
                    onFilter={e => setFilterFuelType(e.target.value)}
                    onClear={() => clearSearch(filterFuelType, setFilterFuelType)}
                    filterText={filterFuelType}
                    label="Brandstof"
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
    }, [filterNumber, filterExpirationDate, filterDriver, filterActive, filterFuelType, filterAvailable, resetPaginationToggle]);

  return (
    <main>
      <ContextHeader breadCrumbs={breadCrumbs} />
      <div className="col-md-12 datatableSection">
            <Button
              type="button"
              className="btn btn-success float-end"
              href={"/fuelCard/Create"}
            >
              <FontAwesomeIcon icon={faPlus} /> Voeg Toe
            </Button>
        <div className="row">
          <div className="col-md-12">
            <DataTable
              className="datatable"
              title="Tankkaarten"
              pagination
              highlightOnHover
              columns={FuelcardColumns}
              data={filteredItems}
              progressPending={pending}
              progressComponent={<Spinner animation={"border"} />}
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
    </main>
  );
};
export default Index;
