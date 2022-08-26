const baseUri = "http://192.168.1.130/api/cars";

async function getCars() {
  queryString = _buildQueryString();
  queryUri = baseUri + queryString;

  try {
    const response = await fetch(queryUri, {
      method: "GET",
      headers: {
        Accept: "application/json",
      },
    });
    const data = await response.json();
    console.log(data);
    _displayCars(data);
  } catch (error) {
    console.log(error);
  } finally {
    _clearSearchForm();
  }
}

function _buildQueryString() {
  const year = document.getElementById("year").value.trim();
  const make = document.getElementById("make").value.trim();
  const model = document.getElementById("model").value.trim();
  const color = document.getElementById("color").value.trim();

  searchParams = new URLSearchParams();
  if (year) searchParams.append("year", year);
  if (make) searchParams.append("make", make);
  if (model) searchParams.append("model", model);
  if (color) searchParams.append("color", color);

  return [...searchParams].length ? `?${searchParams}` : "";
}

function _clearSearchForm() {
  ids = ["year", "make", "model", "color"];
  for (const id of ids) {
    document.getElementById(id).value = "";
  }
}

function _displayCars(data) {
  const tBody = document.getElementById("cars");
  tBody.innerHTML = "";

  data.forEach((car) => {
    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    td1.appendChild(document.createTextNode(car.year));

    let td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(car.make));

    let td3 = tr.insertCell(2);
    td3.appendChild(document.createTextNode(car.model));

    let td4 = tr.insertCell(3);
    td4.appendChild(document.createTextNode(car.color));

    let td5 = tr.insertCell(4);
    td5.appendChild(document.createTextNode(car.vin));
  });

  document.getElementById("carsTable").style.display = data.length
    ? "inline-block"
    : "none";
}
