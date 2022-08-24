const uri = "https://localhost:7188/api/cars";

async function getCars() {
  const year = document.getElementById("year").value.trim();
  const make = document.getElementById("make").value.trim();
  const model = document.getElementById("model").value.trim();
  const color = document.getElementById("color").value.trim();

  getUri = uri;
  searchParams = new URLSearchParams();
  if (year) searchParams.append("year", year);
  if (make) searchParams.append("make", make);
  if (model) searchParams.append("model", model);
  if (color) searchParams.append("color", color);

  if ([...searchParams].length) getUri += `?${searchParams}`;

  try {
    const response = await fetch(getUri, {
      method: "GET",
      headers: {
        Accept: "application/json",
      },
    });

    const data = await response.json();
    console.log(data);
  } catch (error) {
    console.log(error);
  }

  clearSearchForm();
}

function clearSearchForm() {
  ids = ["year", "make", "model", "color"];
  for (const id of ids) {
    document.getElementById(id).value = "";
  }
}
